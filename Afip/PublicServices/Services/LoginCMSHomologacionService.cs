using Afip.Cache;
using Afip.PublicServices.Interfaces;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using LoginHomologacion;
using System.Security;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using WSFEHomologacion;

namespace Afip.PublicServices.Services
{
    public class LoginCMSHomologacionService : ILoginCMSHomologacionService
    {
        private readonly IMasterClassService _masterClassService;
        private readonly IMasterService _masterService;



        #region Private Properties

        private XmlDocument XmlLoginTicketRequest = null;
        private XmlDocument XmlLoginTicketResponse = null;
        private readonly string XmlStrLoginTicketRequestTemplate = "<loginTicketRequest><header><uniqueId></uniqueId><generationTime></generationTime><expirationTime></expirationTime></header><service></service></loginTicketRequest>";
        private UInt32 _globalUniqueID = 0;
        private static readonly string Folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Certificados");
        private Billing Billing { get; set; }
        private CommerceData CommerceData { get; set; }


        #endregion


        public LoginCMSHomologacionService(IMasterClassService masterClassService, IMasterService masterService)
        {
            _masterClassService = masterClassService;
            _masterService = masterService;
        }

        public async Task<AfipLoginResponse> LogInWSFEAsync()
        {
            try
            {
                SecureString passwordSecureString = new();
                string certificatePath;
                if (!AfipCache.Instance.HasData)
                {
                    AfipCache.Reading = true;
                    CommerceData? commerceData = await _masterClassService.GetCommerceDataAsync();
                    if (commerceData != null)
                    {
                        if (commerceData.Billings != null && commerceData.Billings.Count() > 0)
                        {

                            AfipCache.Instance.SetData(commerceData, commerceData.Billings.First());
                        }
                    }
                    AfipCache.Reading = false;
                }

                Billing = AfipCache.Instance.GetBilling();
                CommerceData = AfipCache.Instance.GetCommerceData();


                if (Billing != null && Billing.WSDLExpirationTime > DateTime.Now)
                {

                    return new AfipLoginResponse
                    {
                        Success = true,
                        Object = new FEAuthRequest
                        {
                            Cuit = CommerceData.CUIT,
                            Sign = Billing.WSDLSign,
                            Token = Billing.WSDLToken,
                        },
                    };
                }
                else
                {
                    certificatePath = Path.Combine(Folder, string.Format("{0}.pfx", AfipCache.Instance.GetCommerceData().CUIT));
                    string pass = CryptoHelper.Decrypt(Billing.CertPass);
                    foreach (char c in pass)
                        passwordSecureString.AppendChar(c);
                    passwordSecureString.MakeReadOnly();


                    AfipLoginResquestResponse resultLogIn = await ObtenerLoginTicketResponseAsync(certificatePath, passwordSecureString);

                    if (resultLogIn.Success)
                    {
                        Billing.WSDLGenerationTime = resultLogIn.GenerationTime;
                        Billing.WSDLExpirationTime = resultLogIn.ExpirationTime;
                        Billing.WSDLSign = resultLogIn.Sign;
                        Billing.WSDLToken = resultLogIn.Token;
                        Billing.UniqueId = resultLogIn.UniqueId;
                        AfipCache.Instance.SetData(CommerceData, Billing);
                        await _masterService.UpdateAsync(Billing);
                        return new AfipLoginResponse
                        {
                            Success = true,
                            Object = new FEAuthRequest
                            {
                                Cuit = CommerceData.CUIT,
                                Sign = Billing.WSDLSign,
                                Token = Billing.WSDLToken,
                            },
                        };

                    }
                    return new AfipLoginResponse
                    {
                        Success = false,
                        Message = resultLogIn.Message
                    };

                }
            }
            catch (Exception ex)
            {
                return new AfipLoginResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }




        #region Private Methods

        private byte[] FirmaBytesMensaje(byte[] argBytesMsg, X509Certificate2 argCertFirmante)
        {
            try
            {
                // Pongo el mensaje en un objeto ContentInfo (requerido para construir el obj SignedCms)
                ContentInfo infoContenido = new ContentInfo(argBytesMsg);
                SignedCms cmsFirmado = new SignedCms(infoContenido);

                // Creo objeto CmsSigner que tiene las caracteristicas del firmante
                CmsSigner cmsFirmante = new CmsSigner(argCertFirmante)
                {
                    IncludeOption = X509IncludeOption.EndCertOnly
                };



                // Firmo el mensaje PKCS #7
                cmsFirmado.ComputeSignature(cmsFirmante);



                // Encodeo el mensaje PKCS #7.
                return cmsFirmado.Encode();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        private async Task<AfipLoginResquestResponse> ObtenerLoginTicketResponseAsync(string certificatePath, SecureString certificatePassword)
        {
            string cmsFirmadoBase64 = null;
            string loginTicketResponse = null;

            AfipLoginResquestResponse ticketResponse = new AfipLoginResquestResponse { Success = false, };

            // PASO 1: Genero el Login Ticket Request
            try
            {
                _globalUniqueID += 1;

                XmlLoginTicketRequest = new XmlDocument();
                XmlLoginTicketRequest.LoadXml(XmlStrLoginTicketRequestTemplate);

                XmlNode xmlNodoUniqueId = XmlLoginTicketRequest.SelectSingleNode("//uniqueId");
                XmlNode xmlNodoGenerationTime = XmlLoginTicketRequest.SelectSingleNode("//generationTime");
                XmlNode xmlNodoExpirationTime = XmlLoginTicketRequest.SelectSingleNode("//expirationTime");
                XmlNode xmlNodoService = XmlLoginTicketRequest.SelectSingleNode("//service");
                xmlNodoGenerationTime.InnerText = DateTime.Now.AddMinutes(-10).ToString("s");
                xmlNodoExpirationTime.InnerText = DateTime.Now.AddMinutes(+10).ToString("s");
                xmlNodoUniqueId.InnerText = Convert.ToString(_globalUniqueID);
                xmlNodoService.InnerText = "wsfe";


            }
            catch (Exception excepcionAlGenerarLoginTicketRequest)
            {
                ticketResponse.Message = string.Format("{0} {1} {2}",
                    "***Error GENERANDO el LoginTicketRequest:", excepcionAlGenerarLoginTicketRequest.Message,
                    excepcionAlGenerarLoginTicketRequest.StackTrace);
                return ticketResponse;
            }

            // PASO 2: Firmo el Login Ticket Request
            try
            {

                X509Certificate2 certFirmante = new X509Certificate2(certificatePath, certificatePassword, X509KeyStorageFlags.MachineKeySet);

                DateTime certExpire = DateTime.Parse(certFirmante.GetExpirationDateString());
                if (certExpire < DateTime.Now)
                    return new AfipLoginResquestResponse
                    {
                        Success = false,
                        Message = "Certificado Expirado",
                    };
                else if (certExpire.Date < DateTime.Now.Date.AddDays(60))
                {
                    Billing.ExpireCertificate = true;
                    Billing.ExpireCertificateText = string.Format("El Certificado de AFIP expira en el día: {0:dd/MM/yyyy}", certExpire);
                }
                else
                {
                    Billing.ExpireCertificate = false;
                    Billing.ExpireCertificateText = string.Empty;
                }


                // Convierto el Login Ticket Request a bytes, firmo el msg y lo convierto a Base64
                Encoding EncodedMsg = Encoding.UTF8;

                byte[] msgBytes = EncodedMsg.GetBytes(XmlLoginTicketRequest.OuterXml);
                if (msgBytes == null)
                {
                    ticketResponse.Message = "msgBytes Viene NULL ";
                    return ticketResponse;
                }

                byte[] encodedSignedCms = FirmaBytesMensaje(msgBytes, certFirmante);


                if (encodedSignedCms == null)
                {
                    ticketResponse.Message = "encodedSignedCms Viene NULL ";
                    return ticketResponse;
                }


                cmsFirmadoBase64 = Convert.ToBase64String(encodedSignedCms);

            }
            catch (Exception excepcionAlFirmar)
            {
                ticketResponse.Message = string.Format("{0} {1} ", "***Error al firmar el Login de AFIP. ", excepcionAlFirmar.Message/*, certificatePath*/);
                return ticketResponse;
            }

            // PASO 3: Invoco al WSAA para obtener el Login Ticket Response
            try
            {

                LoginCMSClient servicioWsaa = new();

                loginCmsResponse resultLogin = await servicioWsaa.loginCmsAsync(cmsFirmadoBase64);


                loginTicketResponse = resultLogin.loginCmsReturn;

            }
            catch (Exception ex)
            {
                ticketResponse.Message = string.Format("{0} {1}", "***Error INVOCANDO al servicio WSAA: ", ex.Message);
                return ticketResponse;
            }

            // PASO 4: Analizo el Login Ticket Response recibido del WSAA
            try
            {
                XmlLoginTicketResponse = new XmlDocument();
                XmlLoginTicketResponse.LoadXml(loginTicketResponse);

                ticketResponse.UniqueId = UInt32.Parse(XmlLoginTicketResponse.SelectSingleNode("//uniqueId").InnerText);
                ticketResponse.GenerationTime = DateTime.Parse(XmlLoginTicketResponse.SelectSingleNode("//generationTime").InnerText);
                ticketResponse.ExpirationTime = DateTime.Parse(XmlLoginTicketResponse.SelectSingleNode("//expirationTime").InnerText);
                ticketResponse.Sign = XmlLoginTicketResponse.SelectSingleNode("//sign").InnerText;
                ticketResponse.Token = XmlLoginTicketResponse.SelectSingleNode("//token").InnerText;
                ticketResponse.Success = true;
            }
            catch (Exception excepcionAlAnalizarLoginTicketResponse)
            {
                ticketResponse.Message = string.Format("{0} {1}", "***Error ANALIZANDO el LoginTicketResponse: ", excepcionAlAnalizarLoginTicketResponse.Message);
                return ticketResponse;
            }
            return ticketResponse;
        }


        #endregion
    }
}

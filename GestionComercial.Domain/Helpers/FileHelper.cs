using GestionComercial.Domain.Response;
using GestionComercial.Domain.Statics;

namespace GestionComercial.Domain.Helpers
{
    public class FileHelper
    {
        public static GeneralResponse SaveCertificateAfip(string origin, string name)
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Certificados")))
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Certificados"));
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Certificados", name);
            if (File.Exists(path))
                File.Delete(path);

            try
            {
                File.Copy(origin, path);

                return new GeneralResponse
                {
                    Success = true,
                };

            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public GeneralResponse RemoveCertificateAfip(string name)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Certificados", name);

                if (File.Exists(path))
                    File.Delete(path);

                return new GeneralResponse
                {
                    Success = true,
                };

            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public void RestoreBackUp(string originPath)
        {
            try
            {
                StaticCommon.ContextInUse = true;

                string name = "Gestion.cmp";
                string oldName = "GestionOld.cmp";
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", name);
                string oldPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", oldName);

                if (File.Exists(path))
                {
                    File.Copy(path, oldPath, true);
                }


                File.Copy(originPath, path, true);
                Thread.Sleep(1000);
                StaticCommon.ContextInUse = false;
            }
            catch (Exception)
            {
            }
        }

        public void SaveBackUp(string path)
        {
            try
            {
                StaticCommon.ContextInUse = true;
                string name = "Gestion.cmp";
                string originPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", name);

                File.Copy(originPath, path, true);

                Thread.Sleep(1000);
                StaticCommon.ContextInUse = false;
            }
            catch (Exception)
            {
            }
        }

        public GeneralResponse SaveLogo(string origin)
        {
            try
            {
                if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Images")))
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Images"));
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Images", "Report_logo.png");
                string pathOld = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Images", "Report_logoOLD.png");
                if (File.Exists(path))
                    File.Copy(path, pathOld, true);


                File.Copy(origin, path, true);

                return new GeneralResponse
                {
                    Success = true,
                };

            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public static byte[] FilePathToByteArray(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    FileStream fs = new FileStream(path, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    byte[] imagen = new byte[(int)fs.Length];
                    br.Read(imagen, 0, (int)fs.Length);
                    br.Close();
                    fs.Close();


                    return imagen;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static GeneralResponse SaveByteArrayToFile(byte[] file, string path)
        {
            if (file == null || string.IsNullOrEmpty(path))
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = "Falta File, Path",
                };
            }

            try
            {                
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    foreach (byte b in file)
                    {
                        stream.WriteByte(b);
                    }
                    stream.Close();
                    stream.Dispose();
                }

                return new GeneralResponse
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null
                    && ex.InnerException.InnerException.InnerException != null)
                    return new GeneralResponse
                    {
                        Success = false,
                        Message = ex.InnerException.InnerException.InnerException.Message,
                    };
                else if (ex.InnerException != null && ex.InnerException.InnerException != null)
                    return new GeneralResponse
                    {
                        Success = false,
                        Message = ex.InnerException.InnerException.Message,
                    };
                else
                    return new GeneralResponse
                    {
                        Success = false,
                        Message = ex.Message,
                    };
            }
        }

    }
}

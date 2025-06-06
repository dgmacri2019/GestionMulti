﻿using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IClienService
    {
        Task<GeneralResponse> DeleteAsync(int id);
        Task<IEnumerable<ClientViewModel>> GetAllAsync(bool isEnabled, bool isDeleted);
        Task<IEnumerable<ClientViewModel>> SearchToListAsync(string name, bool isEnabled, bool isDeleted);
        Task<ClientViewModel?> GetByIdAsync(int id, bool isEnabled, bool isDeleted);
    }
}

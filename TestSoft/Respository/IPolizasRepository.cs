using Microsoft.AspNetCore.Mvc;
using TestSoft.Collections;

namespace TestSoft.Respository
{
    public interface IPolizasRepository 
    {
        Task<List<Polizas>> GetAllAsync();
        Task<List<Polizas>> GetAsync();
    }
}

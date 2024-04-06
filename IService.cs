using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Dato;

namespace codemazeapicontroler.Services.Ad
{
     public interface IService
    {
       
        Task<List<Item>> GetAllNotes();
        
        Task<string> CreateNote(Item newnote);
        Task<string> UpdateNote(Item updatenote,int id);
        Task<string> DeleteNote(int id);

    }
}


using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Dato;

namespace codemazeapicontroler.Services.Ad
{
    public class Service:IService
    {
       
        private readonly Mycontext _context;
       

        public Service( Mycontext  context)
        {
          
            _context = context;
           
        }

        public  async Task<string> CreateNote(Item newItem)
        {
            try
            {
                Console.WriteLine("newItem");
                // בדיקה אם newNote או הערכים בתוכו חוקיים
                if (newItem == null)
                {
                    Console.WriteLine("Error: The provided 'newItem' is null.");
                    return null;
                }

             
                // בדיקה נוספת: האם יש גם מידע נוסף שקשור לקטגוריה, שאתה רוצה לבדוק?

                _context.Items.Add(newItem);
                await _context.SaveChangesAsync();

             
                return "the website saccses";
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating a note: {ex.Message}");
                throw;
            }
        }



        public async Task<string> DeleteNote(int id)
        {
            try
            {
                var items = await _context.Items
                             .FirstOrDefaultAsync(c => c.Id == id);
                if (items is null)
                    throw new Exception($"Character with Id '{id}' not found.");

                _context.Items.Remove(items);
                await _context.SaveChangesAsync();

                 return "the website delite sacses";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting a note: {ex.Message}");
                throw;
            }
        }


        public async Task<List<Item>> GetAllNotes()
        {
            try
            {
                var items = await _context.Items
                        .ToListAsync();
                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving all notes: {ex.Message}");
                throw;
            }
        }


      

     

        public async Task<string> UpdateNote(Item updateItem, int id)
        {
            try
            {
                var items = await _context.Items.FirstOrDefaultAsync(c => c.Id == id);
                
               items.Name = updateItem.Name;
               items.IsComplete = updateItem.IsComplete;
               

                await _context.SaveChangesAsync();

                
                return "the website sacses";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating a note: {ex.Message}");
                throw;
            }
        }

        
    }

  
}

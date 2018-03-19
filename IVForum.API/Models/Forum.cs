using IVForum.API.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace IVForum.API.Models
{
	public class Forum
    {
		[Key]
		public Guid Id { get; set; }
		
		[Required]
		[MaxLength(100)]
		public string Title { get; set; }
		[Required]
		[MaxLength(1000)]
		public string Description { get; set; }
		public string Icon { get; set; }
		public string Background { get; set; } = @"http://localhost/assets/images/banner.jpg";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime DateBeginsVote { get; set; } = DateTime.Parse("10/10/1999").Date;
        public DateTime DateEndsVote { get; set; } = DateTime.Parse("10/10/1999").Date;
        public int Views { get; set; } = 0;

        public virtual User Owner { get; set; }
        public virtual List<Wallet> Wallets { get; set; } = new List<Wallet>();
        public virtual List<Project> Projects { get; set; } = new List<Project>();
        public virtual List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public static List<object> ValidateForum(Forum forum)
        {
            List<object> Errors = new List<object>();
            if (forum.Title is null)
            {
                Errors.Add(Message.GetMessage("No s'ha introduit cap títol al forum, Introdueix un títol."));
            }
            if (forum.Description is null)
            {
                Errors.Add(Message.GetMessage("No s'ha introduit cap descripció, introdueix una breu descripció sobre el forum."));
            }
            if (forum.DateBeginsVote == DateTime.Parse("10/10/1999"))
            {
                Errors.Add(Message.GetMessage("No s'ha definit una data per començar les votacions. Defineix una data."));
            }
            if (forum.DateEndsVote == DateTime.Parse("10/10/1999"))
            {
                Errors.Add(Message.GetMessage("No s'ha definit una data per acabar les votacions. Defineix unda data."));
            }
            if (forum.Description.Length > 1000)
            {
                Errors.Add(Message.GetMessage("La llargada de la descripció no pot ser més llarga de 1000 carácters."));
            }
            if (forum.Title.Length > 100)
            {
                Errors.Add(Message.GetMessage("La llargada del títol no pot ser més llarga de 100 carácters."));
            }
            return Errors;
        }
    }
}

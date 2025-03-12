using System;
using System.ComponentModel.DataAnnotations;

namespace ChatRoboteasy.Domain.Enitities
{    
    public class EntidadeUsuario
    {
        [Key]
        public Guid Id { get; set; } 
        public string NomeUsuario { get; set; }
        public string SenhaHash{ get; set; }
    }
}

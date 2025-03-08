using System;

namespace ChatRoboteasy.Domain.Enitities
{    
    public class EntidadeUsuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string NomeUsuario { get; set; }
        public string SenhaHash{ get; set; }
    }
}

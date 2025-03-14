﻿using System.ComponentModel.DataAnnotations;

namespace ChatRoboteasy.Domain.Enitities
{
    public class EntidadeMensagem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Remetente { get; set; }
        public string Destinatario { get; set; }
        public string Conteudo { get; set; }
        public DateTime EnviadaEm { get; set; } = DateTime.UtcNow;
  

}
}

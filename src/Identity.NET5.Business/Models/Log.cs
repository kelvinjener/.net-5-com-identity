using System;

namespace Identity.NET5.Business.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Acao { get; set; }
        public string Descricao { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public string Usuario { get; set; }

    }
}

using System;
using System.Collections.Generic;

namespace AvanadeHealth.Entidades;

public partial class Especialidade
{
    public int IdEspecialidade { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public bool Ativo { get; set; }

   public virtual ICollection<AgendamentoConfiguracao> AgendamentoConfiguracaos { get; } = new List<AgendamentoConfiguracao>();

   public virtual ICollection<Agendamento> Agendamentos { get; } = new List<Agendamento>();
}

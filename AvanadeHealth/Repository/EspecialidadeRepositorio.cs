using AvanadeHealth.Contexto;
using AvanadeHealth.Dto;
using Microsoft.IdentityModel.Tokens;

namespace AvanadeHealth.Repository
{
    public class EspecialidadeRepositorio
    {
        private readonly Context _especialidadeContext;
        public EspecialidadeRepositorio(Context especialidadeContext)
        {
            _especialidadeContext = especialidadeContext;
        }

        public List<Dto.EspecialidadeDto> Especialidades(int id)
        {
            return _especialidadeContext.Especialidades
                               .Where(w => w.IdEspecialidade == id)
                               .Select(s => new EspecialidadeDto()
                               {
                                   IdEspecialidade = s.IdEspecialidade,
                                   Nome = s.Nome,
                                   Descricao = s.Descricao,
                                   Ativo = s.Ativo,
                               })
                               .ToList();
        }
    }
}



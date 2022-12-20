using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text;


namespace AvanadeHealth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadeController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public EspecialidadeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/ListarEspecialidades")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Entidades.Especialidade>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ListarEspecialidades()
        {
            List<Entidades.Especialidade> especialides = new List<Entidades.Especialidade>();

            try
            {
                string strConexao = _configuration.GetConnectionString("SqlHealth");

                using SqlConnection connection = new SqlConnection(strConexao);
                {
                    connection.Open();

                    StringBuilder strComando = new StringBuilder();
                    strComando.AppendLine("SELECT [IdEspecialidade]");
                    strComando.AppendLine("                 ,[Nome]");
                    strComando.AppendLine("                 ,[Descrição]");
                    strComando.AppendLine("                 ,[Ativo]");
                    strComando.AppendLine("     FROM [dbo].[Especialidade]");

                    SqlCommand cmd = new SqlCommand(strComando.ToString(), connection);
                    SqlDataReader retornoSelect = cmd.ExecuteReader();

                    while (retornoSelect.Read())
                    {
                        especialides.Add(new Entidades.Especialidade()
                        {
                            IdEspecialidade = Convert.ToInt32(retornoSelect["IdEspecialidade"] ?? "0"),
                            Nome = TratarNulo(retornoSelect["Nome"]),
                            Descrição = TratarNulo(retornoSelect["Descrição"]),
                            Ativo = Convert.ToBoolean(retornoSelect["Ativo"] == DBNull.Value ? false : retornoSelect["Inativo"])
                        });
                    }
                }
                return Ok(especialides);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        private string TratarNulo(object valor)
        {
            if (valor == DBNull.Value)
                return string.Empty;
            else
                return Convert.ToString(valor);
        }


        [HttpGet]
        [Route("/ListarEspecialidadePorId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Entidades.Especialidade>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ListarEspecialidadePorId(int id)
        {
            try
            {
                return Ok(new Entidades.Especialidade());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("/InserirEspecialidade")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult InserirEspecialidade(Entidades.Especialidade especialidade)
        {
            try
            {
                return Ok(new Entidades.Especialidade());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("/ExcluirEspecialidade/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ExcluirEspecialidade(Entidades.Especialidade especialidade)
        {
            try
            {
                return Ok(new Entidades.Especialidade());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("/AtualizarEspecialidade")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AtualizarEspecialidade(Entidades.Especialidade especialidade)
        {
            try
            {
                return Ok(new Entidades.Especialidade());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

  
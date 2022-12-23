using AvanadeHealth.Entidades;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text;
using static Dapper.SqlMapper;


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
                    strComando.AppendLine("                 ,[Descricao]");
                    strComando.AppendLine("                 ,[Ativo]");
                    strComando.AppendLine("     FROM [dbo].[Especialidade]");

                    SqlCommand cmd = new SqlCommand(strComando.ToString(), connection);
                    SqlDataReader retornoSelect = cmd.ExecuteReader();

                    while (retornoSelect.Read())
                    {
                        especialides.Add(new Entidades.Especialidade()
                        {
                            IdEspecialidade = Convert.ToInt32(retornoSelect["IdEspecialidade"] ?? "0"),
                            Nome = TratarNulo(retornoSelect["NOME"]),
                            Descricao = TratarNulo(retornoSelect["DESCRICAO"]),
                            Ativo = Convert.ToBoolean(retornoSelect["ATIVO"] == DBNull.Value ? false : retornoSelect["ATIVO"])
                        });
                    }
                }
            https://github.com/GrupoAgendamento/AgendamentoHospitalar
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AvanadeHealth.Entidades.Especialidade))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ListarEspecialidadePorId(int id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SqlHealth"));
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@IDESPECIALIDADE", id);

                AvanadeHealth.Entidades.Especialidade especialidade =
                    connection.Query<AvanadeHealth.Entidades.Especialidade>(
                        "SELECT [IDESPECIALIDADE]" +
                        "                 ,[NOME]" +
                        "                 ,[DESCRICAO]" +
                        "                 ,[ATIVO]" +
                        "     FROM [dbo].[Especialidade]" +
                        " WHERE IDESPECIALIDADE = @IDESPECIALIDADE",
                        dynamicParameters
                        ).FirstOrDefault();
                

                if (especialidade == null || especialidade.IdEspecialidade == 0)
                {
                    return NoContent();
                }

                return Ok(especialidade);
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

        public IActionResult InserirEspecialidade(AvanadeHealth.Entidades.Especialidade entidades)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SqlHealth"));

                int linhasAfetadas = connection.Execute(
                    "INSERT INTO [dbo].[Especialidade] " +
                    "([Nome], [Descricao], [Ativo] )" +
                    "  VALUES (@Nome, @Descricao, @Ativo)", entidades);
                return Ok(linhasAfetadas);
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
        public IActionResult ExcluirEspecialidade(int id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SqlHealth"));

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@IdEspecialidade", id);

                int linhasAfetadas = connection.Execute(
                    "DELETE FROM [dbo].[Especialidade] " +
                    "WHERE IDESPECIALIDADE = @IdEspecialidade", dynamicParameters);
                
                return Ok(linhasAfetadas);
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
        public IActionResult AtualizarEspecialidade(AvanadeHealth.Entidades.Especialidade entidades)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SqlHealth"));
                if (entidades.IdEspecialidade == 0)
                {
                    return BadRequest("IdEspecialidade não informado");
                }

                int linhasAfetadas = connection.Execute(
                       "UPDATE [dbo].[Especialidade] " +
                       "             SET[NOME] = @NOME" +
                       "                 ,[DESCRICAO] = @DESCRICAO" +
                       "                 ,[ATIVO] = @ATIVO" +
                       " WHERE IDESPECIALIDADE = @IDESPECIALIDADE", entidades);
                return Ok(linhasAfetadas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
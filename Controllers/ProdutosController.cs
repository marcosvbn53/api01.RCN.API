using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using RCN.API.Data;

namespace RCN.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProdutosController:ControllerBase
    {
        private readonly IProdutoRepository Repositorio;
        public ProdutosController(IProdutoRepository repositorio){
            this.Repositorio = repositorio;
        }


        [HttpPost]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Criar([FromBody] Produto produto){
            if(produto.Codigo == "")
                return BadRequest("Código do produto não informado !");

            if(string.IsNullOrEmpty(produto.Descricao))
                return BadRequest("Descrição do produto não informado!");
            Repositorio.Inserir(produto);
            return Created(nameof(Criar),produto);
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [ResponseCache(Duration=30)]
        [ProducesResponseType((int)HttpStatusCode.OK)]        
        //[Produces("application/json","application/xml")]
        [Produces("application/xml","application/json")]
        public IActionResult Obter(){
            var list = Repositorio.Obter();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult Obter(int id){            
            var produto = Repositorio.Obter(id);
            if(produto == null) return NotFound();
            return Ok(produto);
        }
        
        [HttpPut]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public IActionResult Atualizar([FromBody] Produto produto){            
            var prod = Repositorio.Obter(produto.Id);
            if(prod == null)return NotFound();
            if(string.IsNullOrEmpty(produto.Codigo))
                return BadRequest("Código do produto não informado!");

            if(string.IsNullOrEmpty(produto.Descricao))
                return BadRequest("Descrição do produto não informada!");

            

            Repositorio.Editar(produto);
            return NoContent();
        }

        [ApiVersion("1.0")]
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Apagar(int id){
            var prod = Repositorio.Obter(id);
            if(prod == null)return NotFound();
            Repositorio.Excluir(prod);            
            return Ok();
        }

        [HttpGet("{codigo}")]
        [ApiVersion("2.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult ObterPorCodigo(string codigo){
            return Ok("Método obter por codigo - versão 2");
        }


        [HttpGet("")]
        [ApiVersion("3.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult ObterTodos(){
            List<string> lista = new List<string>();
            for(int i = 0; i < 1000; i++){
                lista.Add($"Indice{i}");
            }
            return Ok(string.Join(",",lista));
        }

    }
}
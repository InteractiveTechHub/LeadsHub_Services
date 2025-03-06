

using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace LeadsHub.Core.Dtos
{
    public sealed class CNPJDataDto
    {
        public string Cnpj { get; set; } = string.Empty;

        [JsonPropertyName("razao_social")]
        public string RazaoSocial { get; set; } = string.Empty;

        [JsonPropertyName("nome_fantasia")]
        public string NomeFantasia { get; set; } = string.Empty;

        [JsonPropertyName("ddd_telefone_1")]
        public string Telefone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string CEP { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Neighborhood { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;
    }
}

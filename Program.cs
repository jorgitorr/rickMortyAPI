using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string characterName = "Summer"; // Nombre del personaje que deseas buscar
        string url = $"https://rickandmortyapi.com/api/character/?name={characterName}";

        try
        {
            // Realiza la solicitud HTTP y espera la respuesta.
            string jsonResponse = await GetApiResponse(url);

            // Deserializa la respuesta JSON en objetos C#.
            var characterResponse = JsonSerializer.Deserialize<CharacterResponse>(jsonResponse);

            if (characterResponse.results.Length > 0)
            {
                // Obtiene el primer resultado (asumiendo que hay solo un personaje con ese nombre).
                var summerCharacter = characterResponse.results[0];

                // Imprime información sobre Summer.
                Console.WriteLine($"Información sobre {characterName}:");
                Console.WriteLine($"ID: {summerCharacter.id}");

                // Imprime los episodios en los que aparece Summer.
                Console.WriteLine($"Episodios en los que aparece {characterName}:");
                foreach (var episodeUrl in summerCharacter.episode)
                {
                    string episodeJson = await GetApiResponse(episodeUrl);
                    var episode = JsonSerializer.Deserialize<Episode>(episodeJson);
                    Console.WriteLine($"- {episode.name} (Episodio {episode.episode})");
                }
                Console.WriteLine("Numeros de episodeos en los que sale Summer: " +summerCharacter.episode.Length);
            }
            else
            {
                Console.WriteLine($"No se encontró ningún personaje con el nombre {characterName}.");
            }

            //Console.WriteLine($"Número total de personajes: {characterResponse.info.count}");
            /*foreach (var character in characterResponse.results)
            {
                //Console.WriteLine($"Nombre: {character.name}, Especie: {character.species}, Estado: {character.status}");
                if(character.name.Equals("Summer Smith"))
                {
                    Console.WriteLine(character.name);
                }
            }*/
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static async Task<string> GetApiResponse(string apiUrl)
    {
        using (var httpClient = new HttpClient())
        {
            // Realiza la solicitud GET a la API y obtiene la respuesta como una cadena JSON.
            var response = await httpClient.GetStringAsync(apiUrl);
            return response;
        }
    }
}

// Clases para deserializar la respuesta JSON.
public class CharacterResponse
{
    public Character[] results { get; set; }
}

public class Character
{
    public int id { get; set; }
    public string name { get; set; }
    public string[] episode { get; set; }
}

public class Episode
{
    public string name { get; set; }
    public string episode { get; set; }
}


using System;
using Impinj.OctaneSdk; // Namespace do SDK da Impinj

class Program
{
    static void Main(string[] args)
    {
        // Troque pelo IP ou hostname do seu leitor R700
        string hostname = "10.103.248.132";

        // Instância do leitor
        var reader = new ImpinjReader();

        try
        {
            Console.WriteLine("Conectando ao leitor...");
            reader.Connect(hostname);

            Console.WriteLine($"Conectado ao leitor {reader.Address}");

            // Evento chamado quando há reporte de tags
            reader.TagsReported += (ImpinjReader sender, TagReport report) =>
            {
                foreach (Tag tag in report)
                {
                    Console.WriteLine($"EPC: {tag.Epc}, Antena: {tag.AntennaPortNumber}, RSSI: {tag.PeakRssiInDbm} dBm");
                }
            };

            // Carrega configurações padrão
            var settings = reader.QueryDefaultSettings();

            // Configura o report
            settings.Report.Mode = ReportMode.Individual;
            settings.Report.IncludeAntennaPortNumber = true;
            settings.Report.IncludePeakRssi = true;

            // Aplica as configurações
            reader.ApplySettings(settings);

            // Inicia a leitura
            Console.WriteLine("Iniciando leitura...");
            reader.Start();

            Console.WriteLine("Pressione ENTER para encerrar...");
            Console.ReadLine();

            // Para a leitura e desconecta
            reader.Stop();
            reader.Disconnect();
            Console.WriteLine("Leitura encerrada.");
        }
        catch (OctaneSdkException e)
        {
            Console.WriteLine($"Erro no Octane SDK: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro: {e.Message}");
        }
    }
}
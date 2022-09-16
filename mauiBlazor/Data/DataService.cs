using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Grpc.Core;
using Bases.BlazorSharedBases.Protos;
using Mapster;

namespace mauiBlazor.Data
{
    public class DataService : ObservableObject
    {
        public List<DataKaryawan> DataKaryawan= new List<DataKaryawan>();
        public DataKaryawan karyawanDipilih { get; set; } = new DataKaryawan();
        public DataService()
        {
        }

        public async Task SetDataDipilih(DataKaryawan karyawan)
        {
            karyawanDipilih = karyawan;
        }
        public async Task<List<DataKaryawan>> GetDataKaryawan()
        {
            DataKaryawan.Clear();
            try
            {
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                var channel = GrpcChannel.ForAddress("https://localhost:5001/", new GrpcChannelOptions { HttpClient = httpClient });
                var client = new ReadKaryawanService.ReadKaryawanServiceClient(channel);
                var request = new GetAllKaryawanRequest();
                var reply = client.GetAll(request);
                var hasil = reply.DaftarKaryawan.Adapt<List<DataKaryawan>>();
                return hasil;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return DataKaryawan;
        }

        public async Task InsertKaryawan(DataKaryawan karyawan)
        {
            try
            {
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                var channel = GrpcChannel.ForAddress("https://localhost:5001/", new GrpcChannelOptions { HttpClient = httpClient });
                var client = new WriteKaryawanService.WriteKaryawanServiceClient(channel);
                var request = karyawan.Adapt<WriteKaryawanRequest>();
                var reply = client.InsertKaryawan(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task UpdateKaryawan(DataKaryawan karyawan)
        {
            try
            {
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                var channel = GrpcChannel.ForAddress("https://localhost:5001/", new GrpcChannelOptions { HttpClient = httpClient });
                var client = new WriteKaryawanService.WriteKaryawanServiceClient(channel);
                var request = new UpdateKaryawanRequest(karyawan.Adapt<UpdateKaryawanRequest>());
                var reply = client.UpdateKaryawan(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task DeleteKaryawan(Guid idkaryawan)
        {
            try
            {
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                var channel = GrpcChannel.ForAddress("https://localhost:5001/", new GrpcChannelOptions { HttpClient = httpClient });
                var client = new WriteKaryawanService.WriteKaryawanServiceClient(channel);
                var request = new DeleteKaryawanRequest
                {
                    IdKaryawan = idkaryawan.ToString(),
                };
                var reply = client.DeleteKaryawan(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public IAsyncEnumerable<proKaryawan> GetData()
        {
            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpClient = httpClient });
            var client = new ReadKaryawanService.ReadKaryawanServiceClient(channel);
            var request = client.GetAllStream(new GetAllKaryawanRequest());
            return request.ResponseStream.ReadAllAsync();
        }
    }
}

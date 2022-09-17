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

        private ReadKaryawanService.ReadKaryawanServiceClient _readClient { get; set; }
        private WriteKaryawanService.WriteKaryawanServiceClient _writeClient { get; set; }

        public DataService()
        {
            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            var channel = GrpcChannel.ForAddress("https://localhost:5001/", new GrpcChannelOptions { HttpClient = httpClient });
            _readClient = new ReadKaryawanService.ReadKaryawanServiceClient(channel);
            _writeClient = new WriteKaryawanService.WriteKaryawanServiceClient(channel);
        }
        
        public async Task<List<DataKaryawan>> GetDataKaryawan()
        {
            var request = new GetAllKaryawanRequest();
            var reply = _readClient.GetAll(request);
            return reply.DaftarKaryawan.Adapt<List<DataKaryawan>>();
        }

        public async Task<bool> InsertKaryawan(DataKaryawan karyawan)
        {
            var request = karyawan.Adapt<WriteKaryawanRequest>();
            var reply = await _writeClient.InsertKaryawanAsync(request);
            return reply.IsOK;
        }

        public async Task<bool> UpdateKaryawan(DataKaryawan karyawan)
        {
            var request = karyawan.Adapt<UpdateKaryawanRequest>();
            var reply = await _writeClient.UpdateKaryawanAsync(request);
            return reply.IsOK;
        }

        public async Task<bool> DeleteKaryawan(Guid idkaryawan)
        {
            var request = new DeleteKaryawanRequest{ IdKaryawan = idkaryawan.ToString()};
            var reply = await _writeClient.DeleteKaryawanAsync(request);
            return reply.IsOK;
            
        }
        public IAsyncEnumerable<proKaryawan> GetData()
        {
            var request = _readClient.GetAllStream(new GetAllKaryawanRequest());
            return request.ResponseStream.ReadAllAsync();
        }
    }
}

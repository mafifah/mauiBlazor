using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Grpc.Core;
using Mapster;
using Shared.Protos;
using static Shared.Protos.svpReadKaryawan;
using static Shared.Protos.svpWriteKaryawan;

namespace mauiBlazor.Data
{
    public class DataService : ObservableObject
    {
        public List<uioT1Karyawan> DataKaryawan= new List<uioT1Karyawan>();

        private svpReadKaryawanClient _readClient { get; set; }
        private svpWriteKaryawanClient _writeClient { get; set; }

        public DataService()
        {
            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            var channel = GrpcChannel.ForAddress("https://localhost:5001/", new GrpcChannelOptions { HttpClient = httpClient });
            _readClient = new svpReadKaryawanClient(channel);
            _writeClient = new svpWriteKaryawanClient(channel);
        }
        
        public async Task<List<uioT1Karyawan>> GetDataKaryawan()
        {
            var request = new GetAllKaryawanRequest();
            var reply = _readClient.GetAll(request);
            return reply.DaftarKaryawan.Adapt<List<uioT1Karyawan>>();
        }

        public async Task<bool> InsertKaryawan(uioT1Karyawan karyawan)
        {
            var request = karyawan.Adapt<WriteKaryawanRequest>();
            var reply = await _writeClient.InsertKaryawanAsync(request);
            return reply.IsOK;
        }

        public async Task<bool> UpdateKaryawan(uioT1Karyawan karyawan)
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

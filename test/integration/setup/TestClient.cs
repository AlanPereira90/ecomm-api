using Moq;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;

using src.domain.payment_type.interfaces;
using src.domain.order.interfaces;

namespace test.integration;

public class TestClient
{

  private HttpClient _client;
  private Mock<IPaymentTypeDao> _mockPaymentTypeDao = new Mock<IPaymentTypeDao>();
  private Mock<IOrderDao> _mockOrderDao = new Mock<IOrderDao>();

  public TestClient()
  {

    var application = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
          builder.ConfigureTestServices(services =>
          {
            services.AddScoped<IPaymentTypeDao>(x => _mockPaymentTypeDao.Object);
            services.AddScoped<IOrderDao>(x => _mockOrderDao.Object);
          });
        });

    _client = application.CreateClient();
    _client.BaseAddress = new Uri("http://localhost:3030");
  }

  public Mock<IPaymentTypeDao> PaymentTypeDao
  {
    get { return _mockPaymentTypeDao; }
  }

  public Mock<IOrderDao> OrderDao
  {
    get { return _mockOrderDao; }
  }

  private ByteArrayContent? PreparePayload(Dictionary<string, string>? headers, object? payload)
  {
    var content = JsonConvert.SerializeObject(payload);

    var buffer = System.Text.Encoding.UTF8.GetBytes(content);
    var byteContent = new ByteArrayContent(buffer);

    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

    if (headers != null)
    {
      foreach (var header in headers)
      {
        byteContent.Headers.Add(header.Key, header.Value);
      }
    }

    return byteContent;
  }

  public async Task<HttpResponseMessage> Post(
    string route,
    Dictionary<string, string>? headers = null,
    object? payload = null
  )
  {
    return await _client.PostAsync(route, this.PreparePayload(headers, payload));
  }

  public async Task<HttpResponseMessage> Put(
    string route,
    Dictionary<string, string>? headers = null,
    object? payload = null
  )
  {
    return await _client.PutAsync(route, this.PreparePayload(headers, payload));
  }

  public async Task<HttpResponseMessage> Patch(
    string route,
    Dictionary<string, string>? headers = null,
    object? payload = null
  )
  {
    return await _client.PatchAsync(route, this.PreparePayload(headers, payload));
  }

  public async Task<HttpResponseMessage> Get(string route, Dictionary<string, string>? headers = null)
  {
    if (headers != null)
    {
      foreach (var header in headers)
      {
        _client.DefaultRequestHeaders.Add(header.Key, header.Value);
      }
    }
    return await _client.GetAsync(route);
  }
}

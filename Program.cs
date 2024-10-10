public void ConfigureServices(IServiceCollection services)
{
    // Thêm HttpClient vào dịch vụ
    services.AddHttpClient();

    // Thêm dịch vụ MVC hoặc Controllers
    services.AddControllers();
}

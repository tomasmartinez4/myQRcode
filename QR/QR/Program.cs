using QRCoder;


string policy = "MyPolicy";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policy, build =>
    {
        build.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
        .AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();

app.UseCors(policy);


app.MapGet("/", () => "Hello World!");

app.MapGet("/qr", (string text) =>
{
    var qrGenerator = new QRCodeGenerator();
    var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
    BitmapByteQRCode bitmapByteQRCode = new BitmapByteQRCode(qrCodeData);
    var bitMap = bitmapByteQRCode.GetGraphic(20);

    using var ms = new MemoryStream();
    ms.Write(bitMap);
    byte[] byteImage = ms.ToArray();
    return Convert.ToBase64String(byteImage);

});

app.Run();

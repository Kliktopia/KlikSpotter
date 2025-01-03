CoconaApp.Run((AppParameters parameters) =>
{
    var service = new FileAnalyzerService(parameters);
    service.ProcessFiles();
});

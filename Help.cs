namespace KlikSpotter;

internal class KlikSpotterHelpAttribute: TransformHelpAttribute
{
    public override void TransformHelp(HelpMessage helpMessage, CommandDescriptor command)
    {
        var descSection = (HelpSection)helpMessage.Children.First(x => x is HelpSection section && section.Id == HelpSectionId.Description);
        descSection.Children.Add(new HelpPreformattedText(@"
  ________
 < Hello! >
  --------
         \   ^__^
          \  (oo)\_______
             (__)\       )\/\
                 ||----w |
                 ||     ||
"));

        helpMessage.Children.Add(new HelpSection(
            new HelpHeading("Example:"),
            new HelpSection(
                new HelpParagraph("MyApp --foo --bar")
            )
        ));
    }
}

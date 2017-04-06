# Field Worker Bot - bot application

This is the bot part of the solution. It contains a bot hosted in ASP.NET Web API application.

## Getting started

To run the bot locally you first have to create the [LUIS](https://www.luis.ai/) application. Then you have to train it with the required intents. The credentials of the LUIS application should go to the FieldWorkerBotSettings.Json file in your user folder:
```
{
	"LUIS:AppName":"YourLUISAppName",
	"LUIS:AppId":"YourLUISAppId",
	"LUIS:AppKey":"YourLUISAppKey"
}
```

## Build and Test

The bot application was created using Visual Studio 2017. 
To test it use the [Bot Framework Emulator](https://docs.botframework.com/en-us/tools/bot-framework-emulator/). Run the project and put its localhost url ``http://localhost:xxxx/api/messages`` into the emulator. The key and password are not required for local testing.
//DiscordNET library is required

using Discord;
using Discord.Audio;
using Discord.Commands;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viktor_Discord_Bot
{
    class ViktorBot
    {
        DiscordClient discord;
        CommandService commands;

        Random rand;
        Random random = new Random();
        
        string[] dankMemes;
        string[] askedQuestions;
        string[] coinFlip;
        string[] boobs;


        private ulong id;
        private Task http;



        public Game CurrentGame { get; private set; }
        public ServerPermissions? Administrator { get; private set; }

        public MyBot()
        {
            rand = new Random();

            dankMemes = new string[]
            {
                "memes/meme1.jpg",
                "memes/meme2.jpg",
                "memes/meme3.jpg",
                "memes/meme4.jpg",
                "memes/meme5.jpg",
                "memes/meme6.jpg",
                "memes/meme7.jpg",
                "memes/meme8.jpg",
                "memes/meme9.jpg",
                "memes/meme10.jpg"
            };

            askedQuestions = new string[]
            {
                ":  Jajjebrun!",
                ":  Skulle inte tro det.",
                ":  Bara om Marcus Bruntberg säger så!",
                ":  Så sant som jag heter Stefan!",
                ":  Nej?",
                ":  Tvek på den...",
                ":  Japp!",
                ":  Antagligen.",
                ":  stfu weeb",
                ":  Mitt svar är *ja*.",
                ":  Jag kunde inte uppfatta vad du frågade... Fråga igen.",
                ":  Ja sir...",
                ":  :middle_finger:"
            };

            coinFlip = new string[]
            {
                "Coin/head.png",
                "Coin/tails.png"
            };

            boobs = new string[]
            {
                System.IO.File.ReadAllText(@"C:\Users\Wigge\Desktop\viktrbot\numbers.txt")
            };


            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = log;
            });
            discord.UsingCommands(x =>
            {
                x.PrefixChar = '.';
                x.AllowMentionPrefix = true;
            });
            

            commands = discord.GetService<CommandService>();

            RegisterMemeCommand();
            RegisterPurgeCommand();
            RegisterKickCommand();
            RegisterBanCommand();
            RegisterMarcusCommand();
            RegisterHelpCommand();
            RegisterQuestionCommand();
            RegisterCoinFlipCommand();
            RegisterInfoCommand();
            RegisterAddCommand();
            RegisterCreateRoleCommand();
            RegisterBoobsCommand();
            RegisterRemoveRoleCommand();

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("YOURTOKEN", TokenType.Bot);
            });
        }

        private void RegisterPurgeCommand()
        {
            commands.CreateCommand("purge")
                .Do(async (e) =>
              {
                  if (e.User.ServerPermissions.Administrator == true || e.User.Id == 163307815056703488)
                  {
                      Message[] messagesToDelete;
                      messagesToDelete = await e.Channel.DownloadMessages(100);

                      await e.Channel.DeleteMessages(messagesToDelete);
                  }
              });
        }
        private void RegisterInfoCommand()
        {
            commands.CreateCommand("whatamiplaying")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("You're playing " + e.User.CurrentGame?.Name);
                });
        }

        private void RegisterAddCommand()
        {
            commands.CreateCommand("botlink")
                .Do(async (e) =>
               {
                   await e.Channel.SendMessage("https://discordapp.com/oauth2/authorize?client_id=237284180575256576&scope=bot&permissions=0");
               });
        }

        //Posts a random picture from media.oboobs.ru
        private void RegisterBoobsCommand()
        {
            commands.CreateCommand("boobs")
                .Do(async (e) =>
               {
                   await e.Channel.SendMessage("coming soon hehe");
               });
        }


        private void RegisterCreateRoleCommand()
        {
            commands.CreateCommand("viktor")
                .Do (async (e) =>
                {
                    if (e.User.Id == 163307815056703488)
                    {
                        await e.Server.CreateRole(name: "Viktor", isMentionable: true, permissions: null, color: Color.Purple, isHoisted: true);
                        var Viktor = e.Server.Roles.FirstOrDefault(x => x.Name == "Viktor");
                        await e.User.AddRoles(Viktor);
                    }
                });
        }

        private void RegisterRemoveRoleCommand()
        {
            commands.CreateCommand("delviktor")
            .Do(async (e) =>
       {
           if (e.User.ServerPermissions.Administrator == true || e.User.Id == 163307815056703488)
           {
               var Viktor = e.Server.Roles.FirstOrDefault(x => x.Name == "Viktor");
               await e.User.RemoveRoles(Viktor);
           }
       });
        }

        private void RegisterMemeCommand()
        {
            commands.CreateCommand("meme")
                .Do(async (e) =>
                {
                    int randomMemes = rand.Next(dankMemes.Length);
                    string coolMeme = dankMemes[randomMemes];
                    await e.Channel.SendFile(coolMeme);
                });
        }

        private void RegisterQuestionCommand()
        {
            commands.CreateCommand("ask")
                .Parameter("a", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    int randomAnswerIndex = rand.Next(askedQuestions.Length);
                    string questionToPost = askedQuestions[randomAnswerIndex];
                    await e.Channel.SendMessage(e.User.Mention + questionToPost);
                });
        }

        private void RegisterKickCommand()
        {
            commands.CreateCommand("kick")
     .Parameter("a", ParameterType.Unparsed)
     .Do(async (e) =>
     {
         if (e.User.ServerPermissions.Administrator == true || e.User.Id == 163307815056703488)
         {
             if (e.Message.MentionedUsers.FirstOrDefault() == null)
             {
                 await e.Channel.SendMessage(e.User.Mention + " That's not a valid user!");
             }
             else
             {
                 try
                 {
                     await e.Message.MentionedUsers.FirstOrDefault().Kick();
                     await e.Channel.SendMessage(e.GetArg("Kick") + " was kicked!");
                 }
                 catch (Exception)
                 {
                     await e.Channel.SendMessage(e.User.Mention + " User kicked!");
                 }
             }
         }
     });
        }

        private void RegisterBanCommand()
        {
            commands.CreateCommand("ban")
 .Parameter("a", ParameterType.Unparsed)
 .Do(async (e) =>
 {
     if (e.User.ServerPermissions.Administrator == true || e.User.Id == 163307815056703488)
     {

         if (e.Message.MentionedUsers.FirstOrDefault() == null)
         {
             await e.Channel.SendMessage(e.User.Mention + " That's not a valid user!");
         }
         else
         {
             try
             {
                 await e.Server.Ban(e.Message.MentionedUsers.FirstOrDefault());
                 await e.Channel.SendMessage(e.GetArg("Ban") + " was banned!");
             }
             catch (Exception)
             {
                 await e.Channel.SendMessage(e.User.Mention + " User banned!");
             }
         }
     }
 });
        }

        private void RegisterMarcusCommand()
        {
            commands.CreateCommand("marcus")
                .Do(async (e) =>
               {
                   await e.Channel.SendMessage("bruntberg!");
               }
                );
        }

        private void RegisterHelpCommand()
        {
            commands.CreateCommand("help")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("   **.ban @user** - Bans a user (*admin only*) \n **.kick** - Kicks a user (*admin only*) \n **.purge** - Deletes the 100 latest messages (*admin only*) \n **.meme** - Posts a random meme \n **.marcus** - Bruntberg \n **.ask >question<** - Ask a question \n **.flip** - Flips a coin \n **.whatamiplaying** - The bot tells you what you are playing \n **.botlink** - Provides you with a link to use if you want the bot in your server.");
                }
                );
        }

        private void RegisterCoinFlipCommand()
        {
            commands.CreateCommand("flip")
                .Parameter("a", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    int randomFlipIndex = rand.Next(coinFlip.Length);
                    string coinToPost = coinFlip[randomFlipIndex];
                    await e.Channel.SendFile(coinToPost);
                    await e.Channel.SendMessage(e.User.Mention);
                });
        }

        private void log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

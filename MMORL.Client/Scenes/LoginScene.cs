using Microsoft.Xna.Framework.Input;
using MMORL.Client.Interface;
using MMORL.Client.Renderers;
using System;

namespace MMORL.Client.Scenes
{
    public class LoginScene : Scene
    {
        private readonly UserInterface _userInterface;
        private readonly Game _game;

        public LoginScene(Game game)
        {
            _game = game;
            _userInterface = new UserInterface();

            Button loginButton = new TextButton(100, 100, "Login");
            loginButton.ButtonClickedEvent += OnLoginEvent;
            _userInterface.Add(loginButton);

            Add(new UiRenderer(_userInterface, Camera));
        }

        private void OnLoginEvent(object sender, EventArgs e)
        {
            string username = "test4@man.com";
            string password = "HelloWorld123";
            _game.Connect(username, password);
        }

        public override void Input(Keys key)
        {
        }

        public override void Update(float delta)
        {
            _userInterface.Update(delta);
            base.Update(delta);
        }
    }
}

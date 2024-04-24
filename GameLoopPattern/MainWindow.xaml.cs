using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace GameLoopPattern {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Dictionary<CGameObject, Image> _gameObjects = new Dictionary<CGameObject, Image>();
        CGameLoop _game;
        private SynchronizationContext context = SynchronizationContext.Current;
        public MainWindow() {
            InitializeComponent();
            // Create a new game
            _game = new CGameLoop();
            _game.Draw += _game_Draw;
            // Add a game objects
            AddGameObject(new Image() {
                Width = 50,
                Height = 20,
                Source = new BitmapImage(new Uri("/Images/bomb.png", UriKind.Relative))
            });

            // Start the game
            _game.Start();
        }
        private void _game_Draw(object sender, TimeElapsedEventArgs e) {

            foreach (CGameObject gameObject in _game.GameObjects) {

                Task.Factory.StartNew(() => {
                    context.Post(state => {
                        Image visual = _gameObjects[gameObject];
                        Canvas.SetLeft(visual, gameObject.Position.X);
                        Canvas.SetTop(visual, gameObject.Position.Y);
                        ElapsedTime.Text = $"Frame: {e.Frame.ToString()}, Time : {e.TotalTime.ToString()}  ,  Elapsed Time: {e.TimeElapsed.ToString()}";
                    }, null);
                });
            }
        }

        public void AddGameObject(Image objectVisual) {
            CGameObject gameObject = new CGameObject();
            gameObject.ApplyForceY(50);

            // Add the visual to the canvas
            MyCanvas.Children.Add(objectVisual);
            // Add the visual to the dictionary
            _gameObjects[gameObject] = objectVisual;
            // Add the game object to the game
            _game.AddGameObject(gameObject);
        }
    }
}
namespace SpaceShoot
{
    public class Enemy
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Size { get; private set; } = 30;
       // public BoxView Visual { get; private set; }
       public Image Visual { get; private set; }
        public int Health { get; set; } = 1;
        public int Score { get; private set; } = 10;

        private double velocityX;
        private double velocityY;
        private double speed = 2.0;
        private Random random = new Random();
        private DateTime lastDirectionChange;
        private int directionChangeInterval = 2000;
        private int howManyChanges = 0;
        private bool isBoss = false;
        // Change direction every 2 seconds

        // Creates a new enemy at the specified position.
        // The enemy will have a random initial direction.
        public Enemy(double x, double y, bool boss = false)
        {
            X = x;
            Y = y;
            lastDirectionChange = DateTime.Now;
            // Create visual representation
            /* Visual = new BoxView
             {
                 Color = Colors.Red ,
                 WidthRequest = Size,
                 HeightRequest = Size,
                 CornerRadius = Size/2 // Make it circular
             };*/
            string source;
            if (!boss)
            {
                int which = Random.Shared.Next(1, 4);
                source = $"alien{which}.png";
            }
            else
            {
                Health = 5;
                source = "monster.png";
                Size *= 2;
                isBoss = true;
                speed = 3.5;
                Score = 100;
            }

            Visual = new Image
            {
                Source = source,
                WidthRequest = Size,
                HeightRequest = Size,
            };

            // Set random initial direction
            ChangeDirection();
        }

        // Updates the enemy's position. Should be called every frame.
        // The enemy moves in its current direction and periodically changes direction.
        public void Update(double screenWidth, double screenHeight) {
            // Move in current direction
            X += velocityX;
            Y += velocityY;

            // Bounce off walls
            if (X < Size / 2 || X > screenWidth - Size / 2) {
                velocityX = -velocityX;
                X = Math.Clamp(X, Size / 2, screenWidth - Size / 2);
            }

            if (Y < Size / 2 || Y > screenHeight - Size / 2) {
                velocityY = -velocityY;
                Y = Math.Clamp(Y, Size / 2, screenHeight - Size / 2);
            }

            // Periodically change direction for more interesting movement
            if ((DateTime.Now - lastDirectionChange).TotalMilliseconds > directionChangeInterval) {
                ChangeDirection();
                lastDirectionChange = DateTime.Now;
            }
        }

        // Changes the enemy's direction to a new random direction.
        private void ChangeDirection() {
            // Generate random angle
            double angle = random.NextDouble() * 2 * Math.PI;

            // Convert to velocity components
            velocityX = Math.Cos(angle) * speed;
            velocityY = Math.Sin(angle) * speed;

            if (!isBoss)
            {
                ++howManyChanges;
                if (howManyChanges % 2 == 0)
                {
                    speed += 1;
                }
            }
        }


        // Alternative update method: Makes the enemy move towards a target (like the player).
        // Maybe different types of enemies could use this behaviour.
        // This is not currently used but demonstrates how to create homing enemies.
        public void MoveTowards(double targetX, double targetY) {
            double dx = targetX - X;
            double dy = targetY - Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance > 0) {
                velocityX = (dx / distance) * speed;
                velocityY = (dy / distance) * speed;
            }
        }
    }
}

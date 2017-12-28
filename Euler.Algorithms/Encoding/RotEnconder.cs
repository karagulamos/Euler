namespace Euler.Algorithms.Encoding
{
    public class RotEnconder
    {
        public string Transform(string text)
        {
            var characters = text.ToCharArray();

            for (var idx = 0; idx < characters.Length; idx++)
            {
                if (!char.IsLetter(characters[idx]))
                    continue;

                Rotate(ref characters[idx], _rotations);
            }

            return new string(characters);
        }

        protected static void Rotate(ref char character, byte rotations)
        {
            char addend = 'A', divisor = 'Z';

            if (char.IsLower(character))
            {
                addend = 'a'; divisor = 'z';
            }

            var rotated = (char)(character + rotations);

            if (rotated > divisor)
            {
                rotated = (char)(rotated % divisor + addend - 1);
            }

            character = rotated;
        }

        public static RotEnconder Create(byte rotations)
        {
            return new RotEnconder(rotations);
        }

        protected RotEnconder(byte rotations)
        {
            _rotations = rotations;
        }

        private readonly byte _rotations;
    }
}
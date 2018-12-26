using Blackmitten.Elliot.Backend;

namespace ElliotTests
{
    internal class MockValidator : IMoveValidator
    {
        public bool Validate(Move move, bool doDiags)
        {
            return true;
        }

    }
}
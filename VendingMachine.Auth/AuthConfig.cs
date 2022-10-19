
    namespace VendingMachine.Auth; 

    public class AuthConfig
    {
        private string _hashingSecret;

        public AuthConfig(string hashingSecret)
        {
            this._hashingSecret = hashingSecret;
        }
        public string HashingSecret
        {
            get { return _hashingSecret; }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 16)
                {
                    throw new ArgumentException(
                        "The argument --hashingsecret must be a string at least 16 characters long.");
                }

                _hashingSecret = value;
            }
        }
      
    }
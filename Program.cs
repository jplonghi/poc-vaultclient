
using System.Text;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.Consul;


IAuthMethodInfo authMethod = new TokenAuthMethodInfo( "s.kvsLk9VPTgPEjQu4fYjx4dla" );
var vaultClientSettings = new VaultClientSettings( "http://localhost:8200", authMethod );
vaultClientSettings.UseVaultTokenHeaderInsteadOfAuthorizationHeader = true;
IVaultClient vaultClient = new VaultClient(vaultClientSettings);



//Read Secret
var kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync( path: "consoleApp", mountPoint: "secret" );
foreach ( var item in kv2Secret.Data.Data )
{
    Console.WriteLine( $"{item.Key} - {item.Value}" );
}

//Decrypt values
var decrypt = await vaultClient.V1.Secrets.Transit.DecryptAsync(
    keyName: "myKey",
    decryptRequestOptions: new VaultSharp.V1.SecretsEngines.Transit.DecryptRequestOptions()
    {
        BatchedDecryptionItems = new List<VaultSharp.V1.SecretsEngines.Transit.DecryptionItem>()
        {
            new VaultSharp.V1.SecretsEngines.Transit.DecryptionItem()
            {
                CipherText = "vault:v1:TMpvl6xPddH1PxMlGBAZT0t4ZQTn0FNH+FZX1CaXQhN6K1rEbtaSrbppatk6VUH+rJ/hM5j9mqXiwcU6JNp6XgbxTPJvI8yx5z8sCz+HsOl6lYAaCcYISMWWCjwYbTslBmDpuiFsd8QjabvplrC+VKYArMTpgfEy2yayE1IkROfmL6kDbm56JzlihtWyO2rqr0Laoqc4Q1vL8d8sYDpfIopQdQe1Kcg7CpZRGxVa1Nd0REujH+cecPoctJRj3VfvsUddsAeNupny26GWjdr4vP9kIirlWo0cBPx2TgeBEOsHpz23BQiW13rslEd09bYpTvmQqOEGNBMGMRekdqK52w=="
            }
        }
    }
    );

Console.WriteLine( Base64Decode( decrypt.Data.BatchedResults[ 0 ].Base64EncodedPlainText ) );


//Encrypt Values
var encryptResult = await vaultClient.V1.Secrets.Transit.EncryptAsync( keyName: "myKey",
encryptRequestOptions: new VaultSharp.V1.SecretsEngines.Transit.EncryptRequestOptions()
{
    BatchedEncryptionItems = new List<VaultSharp.V1.SecretsEngines.Transit.EncryptionItem>()
     {
         new VaultSharp.V1.SecretsEngines.Transit.EncryptionItem()
         {
             Base64EncodedPlainText = Base64Encode ("Mi cedula es 1-xxx-yyy" ),

         }
     }

} ); 

Console.WriteLine( encryptResult.Data.BatchedResults[ 0 ].CipherText );





//Decoding Utils
static string Base64Encode( string plainText )
{
    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes( plainText );
    return System.Convert.ToBase64String( plainTextBytes );
}


static string Base64Decode( string base64EncodedData )
{
    var base64EncodedBytes = System.Convert.FromBase64String( base64EncodedData );
    return System.Text.Encoding.UTF8.GetString( base64EncodedBytes );
}
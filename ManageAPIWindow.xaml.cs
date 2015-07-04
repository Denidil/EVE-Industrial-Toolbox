using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.Models;
using eZet.EveLib.EveXmlModule.Models.Account;


namespace EVE_Industrial_Toolbox
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ManageAPIWindow : Window
    {
        public ManageAPIWindow()
        {
            InitializeComponent();
            this.APIKeysDatagrid.DataContext = EVE_IT_GlobalData.API_Keys;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddAPI_Click(object sender, RoutedEventArgs e)
        {

            int KeyId = int.Parse(this.API_ID.Text);
            ApiKey key = EveXml.CreateApiKey(KeyId, this.API_VCode.Text);

            ApiKeyType kType = key.KeyType;
            DateTime Expire = key.ExpiryDate;
            string Description = "";

            switch (kType)
            {
                case ApiKeyType.Character:
                case ApiKeyType.Account:
                    CharacterKey SingleCharKey = key.GetActualKey() as CharacterKey;

                    Description = "Character(s): ";

                    for (int i = 0; i < SingleCharKey.Characters.Count; i++ )
                    {
                        Description += SingleCharKey.Characters[i].CharacterName;

                        if (i < (i - 1))
                            Description += ", ";

                    }
                    break;

                case ApiKeyType.Corporation:
                    CorporationKey corpKey = key.GetActualKey() as CorporationKey;

                    Description = "Corporation: " + corpKey.Corporation.CorporationName;

                    if (corpKey.Corporation.AllianceName != "")
                        Description += " <" + corpKey.Corporation.AllianceName + ">";

                    EveXmlResponse<CharacterList> corpchars = corpKey.GetCharacterList();

                    if (corpchars.Result.Characters.Count != 0)
                    {
                        Description += "   (" + corpchars.Result.Characters[0].CharacterName + ")";
                        
                    }
                    break;
            }


            API_Key_Store nKey = new API_Key_Store(KeyId, this.API_VCode.Text, kType, Expire, Description);


            EVE_IT_GlobalData.API_Keys.Add(nKey);
            EVE_IT_GlobalData.SerializeSettings();
            this.API_VCode.Text = "";
            this.API_ID.Text = "";
        }

        private void KeyID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}

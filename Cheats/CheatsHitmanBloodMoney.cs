﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CheatsForms
{
    public partial class CheatsHitmanBloodMoney : Form
    {
        static string processName = "HitmanBloodMoney";
        Process[] myProcess;
        static int SteamLenght = 6062080;
        int readLenght = 0;
        bool foundProcess = false;
        bool isSteam = false;
        int ActivateAdressRetail = 0x8ACA89;
        int ActivateAdressSteam = 0x8ABA89;
        int SavedFilesValue = 0;

        public CheatsHitmanBloodMoney()
        {
            InitializeComponent();
        }

        private void CheatsHitmanBloodMoney_Load(object sender, EventArgs e)
        {
            CheckIfRunning.Start();
        }

        private void CheatsHitmanBloodMoney_FormClosing(object sender, FormClosingEventArgs e)
        {
            CheckIfRunning.Stop();
        }

        private void CheckIfRunning_Tick(object sender, EventArgs e)
        {
            if (myProcess == null || myProcess.Length == 0)
            {
                myProcess = Process.GetProcessesByName("HitmanBloodMoney");

                if (myProcess.Length != 0)
                {
                    L_GameRunning.Text = "Blood Money is running";
                    L_GameRunning.ForeColor = Color.Green;
                    CheckIfRunning.Interval = 50;
                }
            }


            if (myProcess.Length > 0)
            {
                foundProcess = true;
            }
            else
                foundProcess = false;

            if(foundProcess)
            {
                L_GameRunning.Text = "Blood Money is running";
                L_GameRunning.ForeColor = Color.Green;
            }
            else
            {
                L_GameRunning.Text = "Blood Money is NOT running";
                L_GameRunning.ForeColor = Color.Red;
            }
        }

        private void B_Activate_Click(object sender, EventArgs e)
        {
            if(foundProcess)
            {
                CheckIfSteam();
                if (isSteam)
                    Trainer.WriteByte(myProcess, ActivateAdressSteam, 1);
                else
                    Trainer.WriteByte(myProcess, ActivateAdressRetail, 1);
            }
        }

        private void B_Deactivate_Click(object sender, EventArgs e)
        {
            if (foundProcess)
            {
                CheckIfSteam();
                if (isSteam)
                    Trainer.WriteByte(myProcess, ActivateAdressSteam, 0);
                else
                    Trainer.WriteByte(myProcess, ActivateAdressRetail, 0);
            }
        }

        private void CheckIfSteam()
        {
            readLenght = myProcess[0].MainModule.ModuleMemorySize;

            if (readLenght == SteamLenght)
                isSteam = true;
            else
                isSteam = false;
        }

        private void TB_AmountOfSaves_TextChanged(object sender, EventArgs e)
        {
            var value = 0;
            if (int.TryParse(TB_AmountOfSaves.Text, out value))
            {
                SavedFilesValue = value;
            }
        }

        private void B_ReadSaveFiles_Click(object sender, EventArgs e)
        {
            if(foundProcess)
            {
                SavedFilesValue = Trainer.ReadInteger(myProcess, 0x009B209C);
                TB_AmountOfSaves.Text = SavedFilesValue.ToString();
            }
        }

        private void B_WriteAValue_Click(object sender, EventArgs e)
        {
            if(foundProcess)
            {
                Trainer.WriteInteger(myProcess, 0x009B209C, SavedFilesValue);
            }
        }
    }
}

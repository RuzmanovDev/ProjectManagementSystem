﻿using ProjectManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectManagement
{
    public partial class SearchProjectForm : Form
    {
        private readonly PmContext context;

        public SearchProjectForm()
        {
            this.context = new PmContext();

            InitializeComponent();

            //Set default values to controls
            this.SearchFilterDropDown.SelectedIndex = 0;
            this.ProjectsGV.Visible = false;
            this.dateTimePicker.Visible = false;
            this.ProjectStatusCb.Visible = false;

        }


        private void SearchProjectForm_Load(object sender, EventArgs e)
        {


        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {

            Expression<Func<PROJECT, bool>> searchCriteria = this.GetSearchCriteria();

            var gridData = context.PROJECTS
                                  .Where(searchCriteria)
                                  .Include(x => x.PROJECT_STATUS1)
                                  .Select(x => new ProjectVM()
                                  {
                                      Id = x.PROJECT_ID,
                                      Name = x.PROJECT_NAME,
                                      Client = x.PROJECT_CLIENT,
                                      StartDate = x.PROJECT_BEGIN,
                                      EndDate = x.PROJECT_END,
                                      Status = x.PROJECT_STATUS1.PSTATUS_NAME,
                                      PayPerH = x.PROJECT_PAY_PER_HOUR,
                                  })
                                  .ToList();

            this.ProjectsGV.Visible = true;
            this.ProjectsGV.DataSource = gridData;

          
        }

        private Expression<Func<PROJECT, bool>> GetSearchCriteria()
        {

            Expression<Func<PROJECT, bool>> searchCriteria = (PROJECT project) => true;

            var enteredCriteria = this.SearchFilterTextBox.Text;

            if (string.IsNullOrEmpty(enteredCriteria))
            {
                // if no filter is entered list all projects
                return searchCriteria;
            }

            var selectedCriteriaIndex = this.SearchFilterDropDown.SelectedIndex;
            //0 Код на проект
            //1 Наименование на проект
            //2 Клиент
            //3 Начало на проект
            //4 Край на проекта
            //5 Статус на проект

            switch (selectedCriteriaIndex)
            {
                case 0:
                    searchCriteria = (PROJECT project) => project.PROJECT_ID.ToString().Contains(enteredCriteria);
                    break;
                case 1:
                    searchCriteria = (PROJECT project) => project.PROJECT_NAME.Contains(enteredCriteria);
                    break;
                case 2:
                    searchCriteria = (PROJECT project) => project.PROJECT_CLIENT.Contains(enteredCriteria);
                    break;
                case 3:
                    searchCriteria = (PROJECT project) => project.PROJECT_BEGIN == dateTimePicker.Value;
                    break;
                case 4:
                    searchCriteria = (PROJECT project) => project.PROJECT_END == dateTimePicker.Value;
                    break;
                case 5:
                    searchCriteria = (PROJECT project) => project.PROJECT_STATUS == ProjectStatusCb.SelectedIndex+1;
                    break;
            }
            return searchCriteria;


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                var form = new ProjectDetailsForm();
                form.ShowDialog();
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SearchFilterDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Код на проект
            //Наименование на проект
            //Клиент
            //Начало на проект
            //Край на проекта
            //Статус на проект

            switch (SearchFilterDropDown.SelectedIndex)
            {
                case 0:
                    SearchFilterTextBox.Visible = true;
                    dateTimePicker.Visible = false;
                    ProjectStatusCb.Visible = false;
                    break;
                case 1:
                    SearchFilterTextBox.Visible = true;
                    dateTimePicker.Visible = false;
                    ProjectStatusCb.Visible = false;
                    break;

                case 2:
                    SearchFilterTextBox.Visible = true;
                    dateTimePicker.Visible = false;
                    ProjectStatusCb.Visible = false;
                    break;
                case 3:
                    SearchFilterTextBox.Visible = false;
                    dateTimePicker.Visible = true;
                    ProjectStatusCb.Visible = false;
                    break;
                case 4:
                    SearchFilterTextBox.Visible = false;
                    dateTimePicker.Visible = true;
                    ProjectStatusCb.Visible = false;
                    break;
                case 5:
                    SearchFilterTextBox.Visible = true;
                    dateTimePicker.Visible = false;
                    ProjectStatusCb.Visible = true;
                    break;

                default:
                    SearchFilterTextBox.Visible = true;
                    dateTimePicker.Visible = false;
                    ProjectStatusCb.Visible = false;
                    break;
            }
        }
    }
}

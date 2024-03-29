﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Collections.ObjectModel;
using System.Collections;


namespace SurfaceApplication1
{
    /// <summary>
    /// 7/13/2012
    /// @ Veronica Lin
    /// Interaction logic for PrimerDesigner2.xaml
    /// CURRENT ISSUES:
    /// No checks on fusion sites for empties, duplicates, or compatibility 
    /// - commented them out for new structure of data transfer and didn't have time to fix them
    /// </summary>
    public partial class PrimerDesigner2 : ScatterViewItem
    {
        public static SurfaceWindow1 sw1;
        private PrimerDesigner1 pd1;
        private static List<Sites> _fusionSiteLibrary;
        private UIElement[][] _partSiteSets;

        #region Properties

        public List<Sites> FusionSiteLibrary
        {
            get { return _fusionSiteLibrary; }
            set { _fusionSiteLibrary = value; }
        }

        public UIElement[][] PartSiteSets
        {
            get { return _partSiteSets; }
        }
        #endregion

        #region Constructors
        //Constructor: Primer Designer launched by a Part
        public PrimerDesigner2(Part p) 
        {
            InitializeComponent();
            Sites.pd2 = this;
            Part.pd2 = this;
            L1Module.pd2 = this;
            L2Module.pd2 = this;
            PrimerDesigner1.pd2 = this;
            SurfaceWindow1.pd2 = this;

            PD2_auto.Children.Add(new Sites());
            PD2_manual.Children.Add(new Sites());

            addPartSiteSets(PD2_auto, p);
            addPartSiteSets(PD2_manual, p);

            PD2_auto.Children.Add(new Sites());
            PD2_manual.Children.Add(new Sites());

            matchSites(PD2_auto);
            matchSites(PD2_manual);
        }

        //Constructor: Primer Designer launched by an L1Module
        public PrimerDesigner2(L1Module l1)
        {
            InitializeComponent();
            Sites.pd2 = this;
            Part.pd2 = this;
            L1Module.pd2 = this;
            L2Module.pd2 = this;
            PrimerDesigner1.pd2 = this;

            PD2_auto.Children.Add(new Sites());
            PD2_manual.Children.Add(new Sites());
            foreach (UIElement elem in l1.L1Grid.Children)
            {
                if (elem.GetType() == typeof(Part))
                {
                    Part p = (Part)elem;
                    addPartSiteSets(PD2_auto, p);
                    addPartSiteSets(PD2_manual, p);
                }
            }
            PD2_auto.Children.Add(new Sites());
            PD2_manual.Children.Add(new Sites());

            matchSites(PD2_auto);
            matchSites(PD2_manual);
        }

        //Constructor: Primer Designer launched by an L2Module
        public PrimerDesigner2(L2Module l2)
        {
            InitializeComponent();
            Sites.pd2 = this;
            Part.pd2 = this;
            L1Module.pd2 = this;
            L2Module.pd2 = this;
            PrimerDesigner1.pd2 = this;

            PD2_auto.Children.Add(new Sites());
            PD2_manual.Children.Add(new Sites());
            foreach (UIElement l1 in l2.Children)
            {
                if (l1.GetType() == typeof(L1Module))
                {
                    foreach (UIElement elem in ((L1Module)l1).L1Grid.Children)
                    {
                        if (elem.GetType() == typeof(Part))
                        {
                            Part p = (Part)elem;
                            addPartSiteSets(PD2_auto, p);
                            addPartSiteSets(PD2_manual, p);
                        }
                    }
                }
            }
            PD2_auto.Children.Add(new Sites());
            PD2_manual.Children.Add(new Sites());

            matchSites(PD2_auto);
            matchSites(PD2_manual);
        }

        #endregion

        #region Constructor helpers

        //Constructor helper: adds Parts and their fusion sites to panel
        private void addPartSiteSets(StackPanel parent, Part p)
        {
            Part copy = p.clone();
            String seq1 = copy.SitesList.ElementAt(0).Sequence;
            String seq2 = copy.SitesList.ElementAt(1).Sequence;

            parent.Children.Add(new Sites(seq1));
            copy.ElementMenu.Items.Remove(copy.PD);
            copy.BorderBrush = p.BorderBrush;
            copy.IsManipulationEnabled = false;
            parent.Children.Add(copy);
            parent.Children.Add(new Sites(seq2));
        }

        //Constructor helper: checks for empty fusion sites and matches them to neighboring defined Sites, if any
        private void matchSites(StackPanel pan)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(pan); i++)
            {
                UIElement elem = (UIElement)VisualTreeHelper.GetChild(pan, i);
                
                //If element is Sites and empty, find its neighbor and copy data from it
                if (elem.GetType() == typeof(Sites) && ((Sites)elem).Sequence == "site")
                {
                    int sourceIndex = Sites.neighborSiteIndex(i, pan);
                    Sites copySource = (Sites)VisualTreeHelper.GetChild(pan, sourceIndex);
                    ((Sites)elem).copySitesInfoFrom(copySource);
                }
            }
        }

        
        #endregion

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = this;
            _fusionSiteLibrary = new List<Sites>();
            //_fusionSiteLibrary.Add(new Sites("Site A", "acaa", new SolidColorBrush(Colors.Gold)));
            //_fusionSiteLibrary.Add(new Sites("Site B", "atgc", new SolidColorBrush(Colors.GreenYellow)));
            //Sites tester = new Sites("Site C", "atgg", new SolidColorBrush(Colors.LimeGreen));
            //_fusionSiteLibrary.Add(tester);
            //_fusionSiteLibrary.Add(new Sites("Site D", "gtca", new SolidColorBrush(Colors.Orchid)));
            //_fusionSiteLibrary.Add(new Sites("Site E", "cctg", new SolidColorBrush(Colors.PaleVioletRed)));
            //_fusionSiteLibrary.Add(new Sites("Site F", "aatg", new SolidColorBrush(Colors.Peru)));
            //_fusionSiteLibrary.Add(new Sites("Site G", "ggta", new SolidColorBrush(Colors.RosyBrown)));
            //_fusionSiteLibrary.Add(new Sites("Site H", "catt", new SolidColorBrush(Colors.Thistle)));
            //_fusionSiteLibrary.Add(new Sites("Site I", "gact", new SolidColorBrush(Colors.DeepSkyBlue)));

            _fusionSiteLibrary.Add(new Sites("acaa"));
            _fusionSiteLibrary.Add(new Sites("atgc"));
            _fusionSiteLibrary.Add(new Sites("atgg"));
            _fusionSiteLibrary.Add(new Sites("gtca"));
            _fusionSiteLibrary.Add(new Sites("cctg"));
            _fusionSiteLibrary.Add(new Sites("ggta"));
            _fusionSiteLibrary.Add(new Sites("catt"));
            _fusionSiteLibrary.Add(new Sites("gact"));

            foreach (Sites s in _fusionSiteLibrary)
            {
                PD2_siteLibrary.Items.Add(s);
                s.Center = SurfaceWindow1.SetPosition(s);
            }
        }

        //Enables touch on tabs
        private void TabControl_TouchDown(object sender, TouchEventArgs e)
        {
            TabItem tab = (TabItem)sender;
            PD2_buildTabs.SelectedItem = tab;
            e.Handled = true;
        }

        private List<List<Part>> partList = new List<List<Part>>() { new List<Part>() };

        //Put Parts and Sites into a list. Generate string of sequences
        private void GeneratePrimers_Click(object sender, RoutedEventArgs e)
        {
            if (pd1 != null)
            {
                sw1.SW_SV.Items.Remove(pd1);
                pd1 = null;
            }

            StackPanel activeTab = PD2_manual;
            if (PD2_buildTabs.SelectedIndex == 1) { activeTab = PD2_auto; }

            int i = 0;

            foreach (UIElement elem in activeTab.Children)
            {
                if (elem.GetType() == typeof(Part))
                {
                    //If list (i.e. L1 module) is full and there are still Parts left
                    //Create new list and increment index
                    if (partList.ElementAt(i).Count == 4)
                    {
                        partList.Add(new List<Part>());
                        i++;
                    }
                    int elemIndex = activeTab.Children.IndexOf(elem);
                    Part p = (Part)elem;
                    //p.updateSites(activeTab, elemIndex);
                    p.updateSites(activeTab);
                    Part clone = p.clone();
                    clone.BorderBrush = p.BorderBrush;
                    clone.ElementMenu.Items.Remove(clone.PD);
                    partList.ElementAt(i).Add(clone);

                }
            }


            pd1 = new PrimerDesigner1(partList);
            sw1.SW_SV.Items.Add(pd1);

            #region Old Generate Primers code
            //2D array of each Part and Site sequence, divided into arrays of Parts and flanking sites (i.e. Sites-Parts-Sites)
            

            //int n = activeTab.Children.Count / 3; //Number of Part-Sites sets, not including the two backbone Sites

            //_partSiteSets = new UIElement[n + 2][];

            //Sites firstFS = ((Sites)VisualTreeHelper.GetChild(activeTab, 0)).clone();
            //Sites lastFS = ((Sites)VisualTreeHelper.GetChild(activeTab, activeTab.Children.Count - 1)).clone();
            //_partSiteSets[0] = new UIElement[] { firstFS };
            //_partSiteSets[n + 1] = new UIElement[] { lastFS };

            //for (int i = 0; i < n; i++)
            //{
            //    int j = 3 * i + 1;
            //    Sites site1 = ((Sites)VisualTreeHelper.GetChild(activeTab, j)).clone();
            //    Part p0 = ((Part)VisualTreeHelper.GetChild(activeTab, j + 1));
            //    Part p = p0.clone();
            //    p.BorderBrush = p0.BorderBrush;
            //    p.ElementMenu.Items.Remove(p.PD);
            //    Sites site2 = ((Sites)VisualTreeHelper.GetChild(activeTab, j + 2)).clone();

            //    UIElement[] subArray = new UIElement[] { site1, p, site2 };
            //    _partSiteSets[i + 1] = subArray;
            //}

            ////Check that Sites are not used twice
            //List<String> sitesList = new List<String>();
            ////Build checklist by taking the left Site in each set besides the first, plus the last Site (technically the left Site of the vector)
            //for (int i = 2; i < _partSiteSets.Length; i++)
            //{
            //    sitesList.Add(((Sites)_partSiteSets[i][0]).Sequence);
            //}
            ////Check sitesList for duplicates
            //bool noDupes = true;
            //bool noEmpty = true;
            ////Start by checking sitesList for the first left-hand Site
            //String checkThis = ((Sites)_partSiteSets[1][0]).Sequence;
            ////Break when sitesList has no items left to compare to checkForThis
            //int count = sitesList.Count;
            //while (sitesList.Count() > 0 /*&& noDupes && noEmpty*/)
            //{
            //    noDupes = !(sitesList.Contains(checkThis));
            //    noEmpty = checkThis != "site";
            //    checkThis = sitesList.ElementAt(0);
            //    sitesList.RemoveAt(0);
            //}
            //if (noDupes && noEmpty)
            //{
            //    pd1 = new PrimerDesigner1(_partSiteSets);
            //    sw1.SW_SV.Items.Add(pd1);
            //}
            //else if (noEmpty && !noDupes)
            //{
            //    MessageBox.Show("Couldn't generate primers. Please check for duplicate fusion sites.");
            //}
            //else if (!noEmpty && noDupes)
            //{
            //    MessageBox.Show("Couldn't generate primers. Please check for empty fusion sites.");
            //}
            //else //Both errors present
            //{
            //    MessageBox.Show("Couldn't generate primers. Please check for duplicate and empty fusion sites.");
            //}
            #endregion
        }

        //Moves pd1 to front and moves pd2 back
        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            int tempZ = pd1.ZIndex;
            pd1.ZIndex = ZIndex;
            ZIndex = tempZ;
        }

        //Removes pd2 and pd1 from sw1 and saves Sites data to Part
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel sp = PD2_manual;
            if (PD2_buildTabs.SelectedIndex == 1) sp = PD2_auto;

            foreach (UIElement elem in sp.Children)
            {
                if (elem.GetType() == typeof(Part))
                {
                    ((Part)elem).updateSites(sp);
                }
            }
            sw1.SW_SV.Items.Remove(this);
            if (pd1 != null) sw1.SW_SV.Items.Remove(pd1);
        }

        //Adds an editable fusion site template to the library
        private void siteAdder_Click(object sender, RoutedEventArgs e)
        {
            Sites template = new Sites();
            template.CircleText.IsReadOnly = false;
            template.Height = 50;
            template.Width = 50;
            PD2_siteLibrary.Items.Add(template);
            template.Center = SurfaceWindow1.SetPosition(template);
        }

        //Enables touch on the build tabs
        private void PD2_buildTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tabc = (TabControl)sender;
            //if manual is selected then
            if (tabc.SelectedIndex == 0)
            {
                permMaker.IsEnabled = false;
                permMaker.Visibility = Visibility.Collapsed;
            }
            else
            //if automatic is selected
            {
                permMaker.IsEnabled = true;
                permMaker.Visibility = Visibility.Visible;
            }
        }

        //Store used indices in a list to check
        //Needs more complex checks: first/last sites of L1Ms must be unique, but others only need to be unique within the given L1M
        private List<int> usedSites;
        private void permMaker_Click(object sender, RoutedEventArgs e)
        {
            permMaker.IsEnabled = false;

            try
            {
                //Set usedSites to empty List and clear all Sites if necessary
                usedSites = new List<int>();
                foreach (UIElement elem in PD2_auto.Children)
                {
                    if (elem.GetType() == typeof(Sites))
                    {
                        ((Sites)elem).copySitesInfoFrom(new Sites());
                    }
                }

                foreach (UIElement elem in PD2_auto.Children)
                {
                    //If the element is Sites and an empty placeholder
                    if (elem.GetType() == typeof(Sites))
                    {
                        if (((Sites)elem).Sequence == "site")
                        {
                            //Get a random index and checks if it's already used
                            int randIndex = (new Random()).Next(0, PD2_siteLibrary.Items.Count);
                            while (usedSites.Contains(randIndex))
                            {
                                randIndex = (new Random()).Next(0, PD2_siteLibrary.Items.Count);
                            }

                            //Copy in data from a random Site in the library
                            Sites current = (Sites)elem;
                            current.copySitesInfoFrom((Sites)PD2_siteLibrary.Items.GetItemAt(randIndex));

                            //Double the Site
                            int nextIndex = PD2_auto.Children.IndexOf(current) + 1;
                            Sites next = (Sites)VisualTreeHelper.GetChild(PD2_auto, nextIndex);
                            next.copySitesInfoFrom(current);

                            usedSites.Add(randIndex);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            permMaker.IsEnabled = true;

        }
    }
}

   M S B A M L   `   `   `  ����J  FReportFrontPage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ReportFrontPageReportFrontPage  5      #    ReportFrontPage.BarcodeWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      /localclr-namespace:ReportFrontPage     5      Gvsm9http://schemas.microsoft.com/netfx/2009/xaml/presentation    5      3ui&http://schemas.lepo.co/wpfui/2022/xaml    5        � Title$  BarcodeWindow��5
      $	��450��5      $	��884��5       � WindowState$ 	Maximized=�5       � WindowStyle$
 None=�5      .��5      �  � RowDefinitions 5      �� 5      $��50 �6   �� 5      $��75 �6   �� 5      $��* �6   �� 5      $��40 �6   .+�5      �    PrimaryBrush#���   6	   $��0��5      ]� 5      $��1��6   .+�5      %   ReportFrontPage.AnimatedTextBox  -   5          Name$ reportNameTextBox��5      $��45��5      $	��350��5      $��15q�5      "   AnimatedTextBoxBorderRadius$ 10Q�5           AnimatedTextBoxBackground
 �����    AnimatedTextBoxForeground# [  5"         AnimatedTextBoxFontSize$ 22Q�5#      !	   AnimatedTextBoxPlaceHolder$	 Search Report Name��5$      $
��Left=�5%      �� 5'      $	��200��6   $��45��5(      
����  �  ComboBoxStyle#��[ 5*      .@�5+      �� $��Alice��6   �� 5,      $	��Bob��6   �� 5-      $��Charlie��6   5.      �� 50      -   5        $��Filter��50      $	��100��51      $��	5,15,5,15q�52      53      (  "System.Windows.Controls.DatePicker  55      -   5        $��myDatePicker��5<        calendarStyle
   CalendarStyle#
 [ 55        DatePickerStyle#��[ 56      #��[  57      $��2q�58      $��5q�59      
����  �
�������$	��175��5=         SelectedDateFormat$
 Long=�5>      $��38��5?      5@      � 5B      -   5        $��myGrid��5C      $��2��5B   	   5E      � 5G      $��2��6	    � ColumnDefinitions 5H      �� 5I      $��* �6   �� 5J      $��* �6   .+�5M      � $��0��6
    5N      �� 5O      $	��100 �6   �� 5P      $��* �6   .+�5S      � $��0��6   
�������$��
10,10,10,0q�5U       5V      �� 5W      $��20 �6   �� 5X      $��* �6   .+�5[      �� 
����  ���6   ��  �� ElementName$ autoAdjustPanel�� �� Path$ ActualWidth�5\      � 5^      $��1��6    5_      �� 5`   	   $��* �6   .+�5c      �� $��Under Review��6   
����  �$
��Left=�5e      $��Center=�5f      $��24(�5g      $
��Bold"�5h      5j      5k      �� 5m      -   5        $��underReviewCanvas��5p      $��1��5m      
�������$��5q�5o       J� 	AllowDrop$
 True��5r      $
}�True��5t      5v      5w      � 5y      -   5        $��
reviewGrid��5{      $��1��5y   
   $��2��5z       5|      �� 5}      $	��100 �6   �� 5~      $��* �6   .+�5�      � $��0��6   
�������$��
10,10,10,0q�5�       5�      �� 5�      $��20 �6   �� 5�      $��* �6   .+�5�      �� 
����  ���6   �� $ autoAdjustPanel��$ ActualWidth�5�      � 5�      $��1��6    5�      �� 5�   	   $��* �6   .+�5�      �� $��Review��6   
����  �$
��Left=�5�      $��Center=�5�      $��24(�5�      $
��Bold"�5�      5�      5�      �� 5�      -   5        $��reviewedCanvas��5�      $��5q�5�      
�������$��1��5�      $
 True��5�      $
}�True��5�      5�      5�      5�      � 5�      $��3��6	   .+�5�      �� -   5        $��myButton��5�      $	��100��5�      $��40��5�      $��Right=�5�      $��0,0,5,0q�5�      $	��Add��5�      5�      5�      5�      5�      
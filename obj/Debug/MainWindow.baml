   M S B A M L   `   `   `  ����
  SimXIV    SimXIV.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6F   -   5          � Title$  Chocobro v0.0.3 | CHAOS��5   	   $	��580��6*   $	��400��6:   $	��768��6I   $
��1024��6V   $
��Auto=�6c    � WindowStartupLocation$ CenterScreen=�6�   $��Center=�6�   $��Center=�6�   .��5      � $��0,1,4,-1q�6    � ColumnDefinitions 5   
   �� 5	      �� 5
      $
��Auto �6    � RowDefinitions 5   
   �� 5      $
��751* �6   �� 5      $	��65* �6   .+�5   
   �� -   5         ��Name$ simulateButton��5      $	��Sim��6)   $��Right=�67   $��Center=�6S   $��20��6�   $��1��6�   $��1��6�   $��50��6�   $��0,0,10,0q�6�   �� 5   
   -   5         ��Name$	 job��5      $
��Left=�6    $��	10,51,0,0q�6;   $	��Top=�6N   $	��120��6f   $��22��6r   {�.  .@�5      �� $
��Bard��6   $
��Left=�6*   $	��118��6E   �� 5      $��Archer��6   $
��Left=�6,   $	��118��6G   {�. 6S   �� 5      $��Dragoon��6   $
��Left=�6-   $	��118��6H   {�. 6T   �� 5      $
��Monk��6   $
��Left=�6*   $	��118��6E   {�. 6Q   �� 5      $��Paladin��6   $
��Left=�6-   $	��118��6H   {�.  6T   �� 5      $��Warrior��6   $
��Left=�6-   $	��118��6H   {�.  6T   �� 5      $��
Black Mage��6   $
��Left=�60   $	��118��6K   {�.  �� 
IsSelected . 6h   �� 5      $��Summoner��6   $
��Left=�6.   $	��118��6I   {�. 6U   �� 5      $��Scholar��6   $
��Left=�6-   $	��118��6H   {�.  6T   �� 5      $��
White Mage��6   $
��Left=�60   $	��118��6K   {�. 6W   �� 5      $��Template��6   $
��Left=�6.   $	��118��6I   {�.  6U   5      �� 5   
   $
��Job:��6   $
��Left=�6   $��	10,24,0,0q�6:   $	��Top=�6M   $��26��6e   $��31��6q   o� 5    
   $��22��6   $	��Top=�6   $��2��63   .@�5!      m� $
��File��6   $��22��6%   .@�5"      m� $��About��6   $
��Left=�6*   $	��140��6E   {�.  6Q   m� 5#      -   5         m�Name$ ExitApplication��5#      $
��Exit��64   $
��Left=�6B   $	��140��6]   6i   5$      5%      �� 5&   
   $��
147,27,0,0q�6   $��2��6)   .@�5(      �� $��Console��6    J� RenderTransformOrigin$ 1.78,0.5&�6'   $��57,-2,-61,0q�6H   .��5*      � ���	#FFE5E5E5$��-2q�6.    5+      �� 5,      $	��82* �6+   �� 5-      $
��587* �6+   .+�5/      �� -   5        	 ��Name$	 console��5/   "   
 �� 
IsReadOnly
 . ��. " �� VerticalScrollBarVisibility$
 Auto=�6�    �� SelectionBrush
 �����$��-1q�6�   $��5q�6�   $��2��6�   ��6�   Z�  � 	Resources 50       �� 
TargetType �� Property �� Value��%X   'J�       �� 51      " J�6$   .�52   "   ��  �@��Margin$ 0q�6;   53      54      55      56      57      �� 58      $��Options��6   $ -0.339,0.409&�6'   $��-55,-2,51,0q�6L   .��59      � 
�������$��0q�6*   .+�5:      �� $��Stat Weights:��6   $
��Left=�64   $��	19,10,0,0q�6O   $	��Top=�6b   $��30��6z   �� 5;      -   5        $ statweights��5;      $
��Left=�64   $��
104,12,0,0q�6O   $	��Top=�6c   $	��120��6{   $��22��6�    � ToolTip$O IUsed with a spreadsheet to determine DPS gains/losses with certain stats.��6�   {�.  .@�5<      �� $
��None��6'   $
��Left=�66   $	��118��6Q    . 6]   �� 5=      $��Weapon Damage��6'   $
��Left=�6?   $	��118��6Z   {�. 6f   �� 5>      $��Magic Damage��6'   $
��Left=�6>   $	��118��6Y   �� 5?      $��Strength��6'   $
��Left=�6:   $	��118��6U   �� 5@      $��Intelligence��6'   $
��Left=�6>   $	��118��6Y   �� 5A      $��	Dexterity��6'   $
��Left=�6;   $	��118��6V   �� 5B      $
��Mind��6'   $
��Left=�66   $	��118��6Q   �� 5C      $��Piety��6'   $
��Left=�67   $	��118��6R   �� 5D      $��Accuracy��6'   $
��Left=�6:   $	��118��6U   �� 5E      $
��Crit��6'   $
��Left=�66   $	��118��6Q   �� 5F      $��Determination��6'   $
��Left=�6?   $	��118��6Z   �� 5G      $��Skill Speed��6'   $
��Left=�6=   $	��118��6X   �� 5H      $��Spell Speed��6'   $
��Left=�6=   $	��118��6X   5K      5L      5M      �� 5N      -   5         ��Name$ 
htmlReport��5N      $��Report��6*   $��	6,-2,-6,0q�6:   $��0,0,0,0q�6M   $
��Left=�6g   .��5O      � ���	#FFE5E5E5 J� IsManipulationEnabled . .+�5P      (  "System.Windows.Controls.WebBrowser  -   5          Name$ browser��5P   !   5Q      5R      5S      �� 5T   
   $��WDMG
��6   $
��Left=�6$   $��
18,128,0,0q�6?   $	��Top=�6S   $��0,2,0,0q�6k   �� 5U   
   -	   5         ��Name$	 WEP��5U      $
��Left=�6   $��20��6:   $��
77,128,0,0q�6F    �� TextWrapping$
 Wrap=�6Z   $��48��6n   $	��Top=�6x   $��52��6�   $ 	0.5,0.652&�6�    �� MaxLines$ 1��6�    �� 	MaxLength$ 6��6�   $��0,1,0,0q�6�   �� 5V   
   $��MDMG
��6   $
��Left=�6$   $��
19,151,0,0q�6?   $	��Top=�6S   $��0,2,0,0q�6k   $ -1.126,-0.919&�6}   �� 5W   
   -
   5        $
 MDMG��5W      $
��Left=�6    $��20��6;   $��
77,150,0,0q�6G   $
 Wrap=�6[   $��82��6o   $	��Top=�6y   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5X   
   $��AADMG
��6   $
��Left=�6%   $��
19,173,0,0q�6@   $	��Top=�6T   $��20��6l   $��0,2,0,0q�6x   $��43��6�   �� 5Y   
   -   5        $ AADMG��5Y      $
��Left=�6!   $��20��6<   $��
77,173,0,0q�6H   $
 Wrap=�6\   $��53.76��6p   $	��Top=�6}   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5Z   
   $��AADELAY
��6   $
��Left=�6'   $��
19,195,0,0q�6B   $	��Top=�6V   $��20��6n   $��0,2,0,0q�6z   $��50��6�   �� 5[   
   -   5        $ DELAY��5[      $
��Left=�6!   $��20��6<   $��
77,195,0,0q�6H   $
 Wrap=�6\   $
��3.36��6p   $	��Top=�6|   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5\   
   $
��STR
��6   $
��Left=�6#   $��
19,216,0,0q�6>   $	��Top=�6R   $��20��6j   $��0,2,0,0q�6v   $��20��6�   �� 5]   
   -   5        $	 STR��5]      $
��Left=�6   $��20��6:   $��
77,217,0,0q�6F   $
 Wrap=�6Z   $	��505��6n   $	��Top=�6y   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5^   
   $
��DEX
��6   $
��Left=�6#   $��
19,238,0,0q�6>   $	��Top=�6R   $��20��6j   $��0,2,0,0q�6v   $��22��6�   �� 5_   
   -   5        $	 DEX��5_      $
��Left=�6   $��20��6:   $��
77,239,0,0q�6F   $
 Wrap=�6Z   $	��594��6n   $	��Top=�6y   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5`   
   $
��VIT
��6   $
��Left=�6#   $��
19,260,0,0q�6>   $	��Top=�6R   $��20��6j   $��0,2,0,0q�6v   $��17��6�   �� 5a   
   -   5        $	 VIT��5a      $
��Left=�6   $��20��6:   $��
77,261,0,0q�6F   $
 Wrap=�6Z   $	��380��6n   $	��Top=�6y   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5b   
   $
��INT
��6   $
��Left=�6#   $��
19,282,0,0q�6>   $	��Top=�6R   $��20��6j   $��0,2,0,0q�6v   $��18��6�   �� 5c   
   -   5        $	 INT��5c      $
��Left=�6   $��20��6:   $��
77,282,0,0q�6F   $
 Wrap=�6Z   $	��677��6n   $	��Top=�6y   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5d   
   $
��MND
��6   $
��Left=�6#   $��
19,304,0,0q�6>   $	��Top=�6R   $��20��6j   $��0,2,0,0q�6v   $ 4.305,9.884&�6�   $��28��6�   �� 5e   
   -   5        $	 MND��5e      $
��Left=�6   $��20��6:   $��
77,304,0,0q�6F   $
 Wrap=�6Z   $	��594��6n   $	��Top=�6y   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5f   
   $
��PIE
��6   $
��Left=�6#   $��
19,326,0,0q�6>   $	��Top=�6R   $��20��6j   $��0,2,0,0q�6v   $��16��6�   �� 5g   
   -   5        $	 PIE��5g      $
��Left=�6   $��20��6:   $��
77,326,0,0q�6F   $
 Wrap=�6Z   $	��525��6n   $	��Top=�6y   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5h   
   $��CRIT
��6   $
��Left=�6$   $��
19,348,0,0q�6?   $	��Top=�6S   $��20��6k   $��0,2,0,0q�6w   $��24��6�   �� 5i   
   -   5        $
 CRIT��5i      $
��Left=�6    $��20��6;   $��
77,348,0,0q�6G   $
 Wrap=�6[   $	��543��6o   $	��Top=�6z   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5j   
   $
��DTR
��6   $
��Left=�6#   $��
19,370,0,0q�6>   $	��Top=�6R   $��20��6j   $��0,2,0,0q�6v   $��22��6�   �� 5k   
   -   5        $	 DTR��5k      $
��Left=�6   $��20��6:   $��
77,370,0,0q�6F   $
 Wrap=�6Z   $	��230��6n   $	��Top=�6y   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5l   
   $
��ACC
��6   $
��Left=�6#   $��
19,392,0,0q�6>   $	��Top=�6R   $��20��6j   $��0,2,0,0q�6v   $��23��6�   �� 5m   
   -   5        $	 ACC��5m      $
��Left=�6   $��20��6:   $��
77,392,0,0q�6F   $
 Wrap=�6Z   $	��499��6n   $	��Top=�6y   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5n   
   $��SKSPD
��6   $
��Left=�6%   $��
19,414,0,0q�6@   $	��Top=�6T   $��20��6l   $��0,2,0,0q�6x   $��35��6�   �� 5o   
   -   5        $ SKSPD��5o      $
��Left=�6!   $��20��6<   $��
77,414,0,0q�6H   $
 Wrap=�6\   $	��408��6p   $	��Top=�6{   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5p   
   $��SPSPD
��6   $
��Left=�6%   $��
19,436,0,0q�6@   $	��Top=�6T   $��20��6l   $��0,2,0,0q�6x   $��35��6�   �� 5q   
   -   5        $ SPSPD��5q      $
��Left=�6!   $��20��6<   $��
77,436,0,0q�6H   $
 Wrap=�6\   $	��616��6p   $	��Top=�6{   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5r   
   -   5        $ iterationsinput��5r      $
��Left=�6+   $��22��6F   $��
10,597,0,0q�6R   $
 Wrap=�6f   $	��Top=�6z   $	��120��6�   $
��5000��6�   $5 /Number of times the simulator will run a fight.��6�   �� 5s   
   $��Iterations:��6   $
��Left=�6&   $��
10,575,0,0q�6A   $	��Top=�6U   $��30��6m   $��62��6y   �� 5t   
   -   5        $ fightLengthInput��5t      $
��Left=�6,   $��22��6G   $��
10,640,0,0q�6S   $
 Wrap=�6g   $	��Top=�6{   $	��120��6�   $	��360��6�   $ How long each fight is.��6�   �� 5u   
   $��Fight Length:��6   $
��Left=�6(   $��
10,618,0,0q�6C   $	��Top=�6W   $��26��6o   $��79��6{   � 5v   
   -   5         �Name$ progressBar��5v      $��Right=�6+   $��0,0,20,0q�6G   $��1��6Y   $	��200��6f   $��Center=�6r   $��20��6�   �� 5w   
   $��MaxHP
��6   $
��Left=�6%   $��
19,458,0,0q�6@   $	��Top=�6T   $��20��6l   $��0,2,0,0q�6x   $��42��6�   �� 5x   
   -   5        $ MaxHP��5x      $
��Left=�6!   $��20��6<   $��
77,458,0,0q�6H   $
 Wrap=�6\   $
��5395��6p   $	��Top=�6|   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   �� 5y   
   $��MaxMP
��6   $
��Left=�6%   $��
19,480,0,0q�6@   $	��Top=�6T   $��20��6l   $��0,2,0,0q�6x   $��42��6�   �� 5z   
   -   5        $ MaxMP��5z      $
��Left=�6!   $��20��6<   $��
77,480,0,0q�6H   $
 Wrap=�6\   $
��3662��6p   $	��Top=�6|   $��52��6�   $ 	0.5,0.652&�6�   $ 1��6�   $ 6��6�   $��0,1,0,0q�6�   5}      5~      
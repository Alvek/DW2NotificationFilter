# **DW2 notification filtering mod**
Work in progress.
Mod will allow player to filter all types of notifications and apply selected choise each time this type of notification apears. 


# **For developers:**

## NativeMethods.txt
WinApi related stuff generation file. Check [CsWin32](https://github.com/microsoft/CsWin32) for more information

## Building and debugging project
Add Directory.Build.props file to NotificationFilter with this content:
```
<Project>
    <PropertyGroup>
        <DW2_ROOT>D:\Games\Distant Worlds 2</DW2_ROOT>
    </PropertyGroup>
</Project>
```
where DW2_ROOT variable path is our game folder. This variable used for dependency dll path (Stride, DW2 dlls) and debug profile (exe path)

# **Planed features**
- Export import of filter settings between games
- UI to edit filter settings (add or remove existing)

# **Known problems**
- First alt-tab on filter settings switch to game insted of other window

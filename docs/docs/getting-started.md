# Getting Started

<br>

## Description.
When developing a program's UI, proper settings must be made in advance to apply EasyTechToolUI.

This document provides detailed instructions on how to make these settings, precautions, and solutions to various problems that may arise.

<br>

## Elements used to implement behavior.

~~~
EasyTechToolUI.dll
~~~

<br>

## Hand-on Guide.

<br>

### 1. Prepare the file ‘EasyTechToolUI.dll’
‘EasyTechToolUI.dll’ is available on the release page of EasyTechToolUI’s github repo.

[EasyTechToolUI Releases](https://github.com/HyunsuYu/EasyTechToolUI/releases)

<br><br>
<div align="center">
  <img src="https://github.com/user-attachments/assets/ecce39b6-2f2c-4da5-a2e1-b15bc4da66d8" width="80%">
</div>
<br><br>

You can download the 'EasyTechToolUI.dll' file of the desired version from the various versions available on the release page. If you want detailed debugging, you can also download 'EasyTechToolUI.pdb'.

<br>

### 2. Create a ‘Plugins’ folder under the Assets folder
If you want to add a desired dll file as a plugin to your Unity project, the standard way is to place it under Assets/Plugins/.

If you'd like to find Unity's official documentation on applying plugins to your project, here are some good starting points:

[Integrating third-party code libraries (plug-ins)](https://docs.unity3d.com/Manual/plug-ins.html)

Back to the main topic, in order to apply the plugin to a Unity project, you must first create a Plugins folder under Assets as shown in the image below.

<br><br>
<div align="center">
  <img src="https://github.com/user-attachments/assets/7d2668aa-ae99-43b2-a6da-f03e7319afb7" width="80%">
</div>
<br><br>

### 3. Move the 'EasyTechToolUI.dll' file under the 'Plugins' folder
Finally, place the downloaded 'EasyTechToolUI.dll' under the Plugins folder to complete the plugin application.

<br><br>
<div align="center">
  <img src="https://github.com/user-attachments/assets/876e4b80-8515-40e9-997f-3cdaefac15e9" width="80%">
</div>
<br><br>

## Points to Note.

<br>

### Dependency conflict issue.
One of the most basic issues to consider when implementing a plugin is dependency conflicts.

In some cases, plugin A may reference another plugin B through Z, and if plugins B through Z are not properly prepared when trying to apply A to the development environment, a critical error may occur.

If a dependency conflict occurs, an error log similar to the image below will be output. In the case of the example image, the problem occurred with the dependency called 'com.google.games:gpgs-plugin-support:0+@aar'.

<br><br>
<div align="center">
    <div align="center">
        <img src="https://github.com/user-attachments/assets/e0da6eeb-6387-439a-9d85-db754963c5f8" width="80%">
    </div>
    Image from https://github.com/playgameservices/play-games-plugin-for-unity/issues/2012
</div>
<br><br>

In order to apply EasyTechToolUI to the development environment, it is necessary to apply the prerequisite plugins that EasyTechToolUI refers to to the development environment in advance. You can find out the dependencies of EasyTechToolUI by checking [README.md](https://github.com/HyunsuYu/EasyTechToolUI/blob/master/README.md) in the EasyTechToolUI github repo.

After checking, you can solve the problem by finding the missing plugin and applying it to the development environment.

<br>

### Version conflict issue.
EasyTechToolUI is constantly being reorganized and improved, including maintaining the previous API and adding new features. As a result, if you download and apply a version of the plugin that does not support the feature you want to use, you will naturally not be able to use that feature.

It would be nice if there were appropriate API documentation for each version, but unfortunately, this is difficult due to various practical issues. Therefore, the best method we recommend is to download and apply the latest version of the plugin.

EasyTechToolUI's documentation is always written based on the latest release version, so this may be the most reasonable method for those who want to apply EasyTechToolUI to their projects.
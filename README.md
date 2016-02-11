# PullSite
A C# program that pulls down all html pages (recursively) linked to on the specified URL

This program was originally written on December 8-9, 2014 in Visual Studio 2012. I am uploading it essentially unchanged for now.

###Usage

```
>PullSite <URL>

```

In the working directory, all of the linked pages __that end in ".html"__ will be added. Their names will be replaced with a 4-digit index number. In addition, all .html pages that _they_ link to will also be added. Each link is also modified to textually indicate what "page" it is pointing to.

This allows the user to eliminate all directory hierarchies on the website while still retaining proper links, and also allows the pages to be printed. The user can then identify which page they need to turn to, akin to a "choose your own adventure" book in execution. For example, a link to 0004.html will be prefaced with the words "(Page 4)" in bold.

The program was specifically made to allow websites with many levels of subdirectories to be quickly pulled down and printed out while retaining link "functionality" on paper.

A list of pulled-down URLs is also added as a text file named __listOfPages.txt__ in the same directory.

###Limitations

At present, the program does not
* pull down the initial page specified
* pull down any other kinds of files or alternate names (this includes dynamic content without a filename)
* pull down any embedded files or pages
* detect JavaScript links

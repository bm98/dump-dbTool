dump-dbTool<br>
===============<br>
<br>
dump1090 database utility<br>
<br>
The dump1090-dbTool (tool) derives from a number of input files databases and other 
output which can be used to update the FA and extended versions of the dump1090 Web Interface.
<br>
Geo location files are created within a range of a given location to limit the size and load time of such data.
<br>
Most input files are from publicly available sources and updated regularly at the time of writing. 
<br>
Flight information derived from a free route data file – however the quality is +- … 
<br>
For airways no suitable source with current content could be found (other than professional sources $$$) 
So this data is derived from XPlane11 flight simulator ($) which is kind of recent but quality is unknown.<br>
Such source data is not included here<br>
<br>
dump1090-dbTool: a command line executable to easily create output as needed<br>
See: Doc folder: dump1090-dbTool-GUIDE-V0.9.pdf<br>
<br>
example-data: an exemplary tool data folder containing input and output<br>
 - You may use data from the output db folder to use it in the standard dum1090fa Web Site<br>
 - Make sure to backup the original db folder by e.g. renaming it before deploying data from here<br>
 - Geolocation data files are created around Lat/Lon 47.7/8.15 with a radius of 300nm - may not be directly usable
<br>
dumpDbBrowser: just a testbed for the various data types (don't rely on it)<br>
d1090dataLib: shared code library for reading, conversion and writing<br>
xy-Test: some testing code<br>
<br>
Uses System.Data.SQLite<br>
<br>
<br>
NOTE: THIS _ IS _ VERY _ EARLY _ WORK _ IN _ PROGRESS _ IT _ MAY _ JUST _ BREAK _ AT _ ANY _ TIME ;-)<br>
<br>



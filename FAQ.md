# Frequently Asked Questions

### Why does OpenFileHelper return void?
`OpenFile` functions are designed to do all the work of a menu command that would open
a file. Therefore, it does not return a value for the same reason that a menu command
does not return a function. A menu command would simply change the state of whatever the
UI is representing. In that regard, `OpenFileHelper` has a `FileOpened` event that
the program can hook to.

Further, the `OpenFile` function has the possibility to open multiple files. It
therefore isn't feasible to have a single return object or code for opening multiple
files.
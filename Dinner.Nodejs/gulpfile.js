function defaultTask(cb) {
    // place code for your default task here
    cb();
  }

function html() {
    return src('./source/pug/pages/**/*.pug')
        .pipe(pug({pretty: true}))
        .pipe(dest('./public'));
}

  exports.html = html
  
  exports.default = defaultTask
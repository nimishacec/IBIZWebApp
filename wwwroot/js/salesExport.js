// Simple PDF export using jsPDF (https://github.com/parallax/jsPDF)
// You must include jsPDF in your index.html for this to work:
// <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>

window.downloadPdf = function () {
    if (!window.jspdf || !window.jspdf.jsPDF) {
        alert('jsPDF library not found. Please include jsPDF in your index.html.');
        return;
    }
    var doc = new window.jspdf.jsPDF();
    doc.text('Exported Data', 10, 10);
    // For a real table export, you would need to pass data from Blazor and use doc.autoTable (see jsPDF-AutoTable plugin)
    doc.save('export.pdf');
};
window.downloadCsv = function(columns, rows) {
    let csvContent = '';
    csvContent += columns.join(',') + '\n';
    rows.forEach(row => {
        csvContent += row.map(cell => '"' + (cell ?? '').replace(/"/g, '""') + '"').join(',') + '\n';
    });
    let blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    let link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = 'SalesSummary.csv';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

window.downloadExcel = function(columns, rows) {
    let table = '<table><tr>' + columns.map(c => `<th>${c}</th>`).join('') + '</tr>';
    rows.forEach(row => {
        table += '<tr>' + row.map(cell => `<td>${cell ?? ''}</td>`).join('') + '</tr>';
    });
    table += '</table>';
    let uri = 'data:application/vnd.ms-excel;base64,';
    let base64 = window.btoa(unescape(encodeURIComponent(table)));
    let link = document.createElement('a');
    link.href = uri + base64;
    link.download = 'SalesSummary.xls';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

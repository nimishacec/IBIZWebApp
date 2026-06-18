# PowerShell script to free a port (e.g., 5280)
param(
    [int]$Port = 5280
)

$process = netstat -ano | Select-String ":$Port" | ForEach-Object {
    ($_ -split '\s+')[-1]
} | Select-Object -First 1

if ($process) {
    Write-Host "Killing process with PID $process using port $Port..."
    Stop-Process -Id $process -Force
    Write-Host "Port $Port is now free."
} else {
    Write-Host "No process found using port $Port."
}
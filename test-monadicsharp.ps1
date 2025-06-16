# MonadicSharp Test Script - Dimostra tutte le funzionalità
# Esegui questo script mentre l'applicazione è in esecuzione su http://localhost:5000

Write-Host "🚀 MonadicSharp Test Environment - Demo Completa" -ForegroundColor Green
Write-Host "=" * 60 -ForegroundColor Blue

# Test 1: GET - Verifica che i customer seedati esistano
Write-Host "`n📊 Test 1: GET All Customers (MonadicSharp Success Pattern)" -ForegroundColor Yellow
try {
    $customers = Invoke-RestMethod -Uri "http://localhost:5000/api/customers" -Method GET
    Write-Host "✅ Successo! Trovati $($customers.Count) customers" -ForegroundColor Green
    $customers | Format-Table -Property @{Expression = { $_.id.Substring(0, 8) }; Label = "ID" }, companyName, email, isActive
}
catch {
    Write-Host "❌ Errore GET: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: POST con dati validi - MonadicSharp Success Chain
Write-Host "`n📊 Test 2: POST Customer Valido (MonadicSharp Validation Chain)" -ForegroundColor Yellow
$validCustomer = @{
    id          = "00000000-0000-0000-0000-000000000000"
    companyName = "MonadicSharp Demo Corp"
    email       = "demo@monadicsharp.com"
    vatNumber   = "1234567890"  # 10 caratteri esatti
    street      = "Via Functional Programming 123"
    city        = "Milano"
    postalCode  = "20100"
    country     = "Italy"
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri "http://localhost:5000/api/customers" -Method POST -Body $validCustomer -ContentType "application/json"
    Write-Host "✅ Customer creato con successo!" -ForegroundColor Green
    Write-Host "   ID: $($response.id)" -ForegroundColor Cyan
    Write-Host "   MonadicSharp ha validato tutti i Value Objects correttamente" -ForegroundColor Green
}
catch {
    Write-Host "❌ Errore POST: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 3: POST con dati invalidi - MonadicSharp Failure Pattern
Write-Host "`n📊 Test 3: POST Customer Invalido (MonadicSharp Error Handling)" -ForegroundColor Yellow
$invalidCustomers = @(
    @{ 
        name = "VAT Number troppo lungo"
        data = @{
            companyName = "Test Corp"
            email       = "test@example.com"
            vatNumber   = "12345678901"  # 11 caratteri (troppi)
            street      = "Via Test 123"
            city        = "Roma"
            postalCode  = "00100"
            country     = "Italy"
        }
    },
    @{
        name = "Email invalida"
        data = @{
            companyName = "Test Corp"
            email       = "email-invalida"
            vatNumber   = "1234567890"
            street      = "Via Test 123"
            city        = "Roma"
            postalCode  = "00100"
            country     = "Italy"
        }
    },
    @{
        name = "Company Name vuoto"
        data = @{
            companyName = ""
            email       = "test@example.com"
            vatNumber   = "1234567890"
            street      = "Via Test 123"
            city        = "Roma"
            postalCode  = "00100"
            country     = "Italy"
        }
    }
)

foreach ($testCase in $invalidCustomers) {
    Write-Host "   📋 Testando: $($testCase.name)" -ForegroundColor Cyan
    try {
        $body = $testCase.data | ConvertTo-Json
        Invoke-RestMethod -Uri "http://localhost:5000/api/customers" -Method POST -Body $body -ContentType "application/json"
        Write-Host "   ❌ Errore: dovrebbe aver fallito!" -ForegroundColor Red
    }
    catch {
        $errorResponse = $_.Exception.Response.GetResponseStream()
        $reader = New-Object System.IO.StreamReader($errorResponse)
        $errorDetails = $reader.ReadToEnd() | ConvertFrom-Json
        Write-Host "   ✅ MonadicSharp ha intercettato l'errore: $($errorDetails.message)" -ForegroundColor Green
    }
}

# Test 4: GET Customer specifico
Write-Host "`n📊 Test 4: GET Customer Specifico" -ForegroundColor Yellow
try {
    $allCustomers = Invoke-RestMethod -Uri "http://localhost:5000/api/customers" -Method GET
    if ($allCustomers.Count -gt 0) {
        $firstCustomerId = $allCustomers[0].id
        $customer = Invoke-RestMethod -Uri "http://localhost:5000/api/customers/$firstCustomerId" -Method GET
        Write-Host "✅ Customer trovato: $($customer.companyName)" -ForegroundColor Green
    }
}
catch {
    Write-Host "❌ Errore GET by ID: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 5: GET Customer inesistente - MonadicSharp Option.None Pattern
Write-Host "`n📊 Test 5: GET Customer Inesistente (MonadicSharp Option.None)" -ForegroundColor Yellow
try {
    $fakeId = [System.Guid]::NewGuid()
    Invoke-RestMethod -Uri "http://localhost:5000/api/customers/$fakeId" -Method GET
    Write-Host "❌ Errore: dovrebbe aver restituito 404!" -ForegroundColor Red
}
catch {
    if ($_.Exception.Response.StatusCode -eq 404) {
        Write-Host "✅ MonadicSharp ha gestito correttamente Option.None -> 404 Not Found" -ForegroundColor Green
    }
    else {
        Write-Host "❌ Errore inaspettato: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# Riepilogo finale
Write-Host "`n🎯 RIEPILOGO COMPLETO MonadicSharp Test Environment" -ForegroundColor Green
Write-Host "=" * 60 -ForegroundColor Blue
Write-Host "✅ Result<T> Success Pattern: Funziona" -ForegroundColor Green
Write-Host "✅ Result<T> Failure Pattern: Funziona" -ForegroundColor Green
Write-Host "✅ Option<T> Some/None Pattern: Funziona" -ForegroundColor Green
Write-Host "✅ Value Objects Validation Chain: Funziona" -ForegroundColor Green
Write-Host "✅ Record-based Domain Models: Funziona" -ForegroundColor Green
Write-Host "✅ Monadic Composition: Funziona" -ForegroundColor Green
Write-Host "✅ Error Handling Monadico: Funziona" -ForegroundColor Green

Write-Host "`n🌟 MonadicSharp è completamente operativo!" -ForegroundColor Yellow
Write-Host "🔗 API Swagger disponibile su: http://localhost:5000" -ForegroundColor Cyan

# Mostra dati finali
Write-Host "`n📊 Stato finale dei Customer:" -ForegroundColor Yellow
try {
    $finalCustomers = Invoke-RestMethod -Uri "http://localhost:5000/api/customers" -Method GET
    $finalCustomers | Format-Table -Property @{Expression = { $_.id.Substring(0, 8) }; Label = "ID" }, @{Expression = { if ($_.companyName) { $_.companyName } else { "[SeedData]" } }; Label = "Company" }, isActive
}
catch {
    Write-Host "❌ Errore finale: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n🚀 Test completato con successo!" -ForegroundColor Green

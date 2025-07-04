# ClarusAI - Healthcare AI Intelligence Platform

⚠️ **Important Security Notice**: This is a healthcare application handling Protected Health Information (PHI). Please review our [Security Policy](SECURITY.md) before contributing.

## Overview

ClarusAI is a HIPAA-compliant healthcare AI platform designed for medical imaging analysis. The platform enables clinicians and radiologists to securely upload patient data and medical scans (CT, X-ray, MRI) to receive AI-powered diagnostic insights.

## 🏥 Key Features

- **Secure Medical Image Upload**: DICOM-compliant image processing with encryption
- **AI-Powered Analysis**: Advanced medical image analysis using Microsoft Semantic Kernel
- **HIPAA Compliance**: End-to-end encryption, audit logging, and access controls
- **Role-Based Access**: Different permission levels for administrators, radiologists, clinicians, and technicians
- **Real-time Processing**: Sub-30 second analysis times for medical images
- **Comprehensive Audit Trail**: Full logging for compliance and security monitoring

## 🛡️ Security & Compliance

### HIPAA Compliance
- ✅ Business Associate Agreements (BAAs) support
- ✅ End-to-end encryption (AES-256)
- ✅ Comprehensive audit logging
- ✅ Access controls and user authentication
- ✅ Data minimization and retention policies

### Security Features
- Multi-factor authentication
- Role-based authorization
- Secure file upload with validation
- Encrypted database connections
- Session management with timeout
- Comprehensive input validation

## 🏗️ Architecture

```
ClarusAI Solution Structure:
├── ClarusAI.Core/          # Domain entities, interfaces, DTOs
├── ClarusAI.Business/      # Business logic and services
├── ClarusAI.Data/          # Data access layer and repositories
└── ClarusAI.Web/           # Blazor Server web application
```

### Technology Stack
- **Backend**: .NET 8, C#, Entity Framework Core
- **Frontend**: Blazor Server, Bootstrap, SignalR
- **Database**: SQL Server with encryption
- **AI/ML**: Microsoft Semantic Kernel, Azure OpenAI
- **Cloud**: Azure (App Service, Key Vault, Cognitive Services)
- **Security**: ASP.NET Core Identity, Azure AD

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)
- Azure subscription (for AI services)
- Visual Studio 2022 or VS Code

### Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-org/ClarusAI.git
   cd ClarusAI
   ```

2. **Configure application settings**
   ```bash
   cp ClarusAI.Web/appsettings.example.json ClarusAI.Web/appsettings.Development.json
   ```
   
   Edit `appsettings.Development.json` with your configuration:
   - Database connection string
   - Azure Key Vault URL
   - JWT secret key
   - Azure AI service keys

3. **Database setup**
   ```bash
   dotnet ef database update --project ClarusAI.Data
   ```

4. **Run the application**
   ```bash
   dotnet run --project ClarusAI.Web
   ```

5. **Access the application**
   - Navigate to `https://localhost:5001`
   - Default admin credentials will be created on first run

## 📋 User Roles & Permissions

### Administrator
- Full system access
- User management
- System configuration
- Audit log access

### Radiologist
- Medical image analysis
- Report generation
- Patient data access
- Analysis result management

### Clinician
- Patient data management
- Image upload
- Analysis request submission
- Result viewing

### Technician
- Image upload
- Basic patient data entry
- Limited viewing permissions

### Viewer
- Read-only access to assigned patients
- View analysis results
- No data modification

## 🔐 Security Configuration

### Environment Variables
Set these environment variables for production:

```bash
CLARUSAI_CONNECTION_STRING="Server=...;Database=ClarusAI;Encrypt=true"
CLARUSAI_JWT_SECRET="your-256-bit-secret-key"
CLARUSAI_KEYVAULT_URL="https://your-keyvault.vault.azure.net/"
CLARUSAI_AZURE_CLIENT_ID="your-azure-client-id"
CLARUSAI_AZURE_CLIENT_SECRET="your-azure-client-secret"
```

### File Upload Security
- Maximum file size: 100MB
- Allowed types: DICOM, JPEG, PNG
- Virus scanning enabled
- Files stored outside web root

## 🧪 Testing

### Unit Tests
```bash
dotnet test
```

### Security Tests
```bash
dotnet test --filter Category=Security
```

### Integration Tests
```bash
dotnet test --filter Category=Integration
```

## 📊 Monitoring & Logging

### Application Insights
- Performance monitoring
- Error tracking
- User behavior analytics
- Security event monitoring

### Audit Logging
All PHI access is logged with:
- User identification
- Timestamp
- Action performed
- IP address
- User agent

## 🚀 Deployment

### Azure Deployment
1. Create Azure resources:
   - App Service
   - SQL Database
   - Key Vault
   - Application Insights
   - Cognitive Services

2. Configure CI/CD pipeline
3. Set environment variables
4. Deploy application

### Docker Deployment
```bash
docker build -t clarusai .
docker run -p 8080:80 clarusai
```

## 📚 API Documentation

### Authentication
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "user@example.com",
  "password": "SecurePassword123!"
}
```

### Patient Management
```http
GET /api/patients
Authorization: Bearer <token>
```

### Medical Image Upload
```http
POST /api/medical-images
Authorization: Bearer <token>
Content-Type: multipart/form-data

{
  "patientId": "guid",
  "imageFile": "binary data",
  "imageType": "CT",
  "bodyPart": "Chest"
}
```

## 🤝 Contributing

1. Review our [Security Policy](SECURITY.md)
2. Fork the repository
3. Create a feature branch
4. Implement changes with tests
5. Submit pull request with security review

### Code Standards
- Follow C# coding conventions
- Include comprehensive unit tests
- Security review for all changes
- Documentation updates

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

### Security Issues
Report security vulnerabilities to: security@clarusai.com

### General Support
- GitHub Issues: For bug reports and feature requests
- Documentation: Check our [Wiki](https://github.com/your-org/ClarusAI/wiki)
- Email: support@clarusai.com

## 🔍 Compliance & Certifications

- HIPAA Compliance Ready
- SOC 2 Type II (Planned)
- FDA 510(k) Preparation (Planned)
- ISO 27001 Alignment

## 📈 Roadmap

### Phase 1: Core Platform (Current)
- ✅ Basic patient management
- ✅ Secure image upload
- ✅ User authentication
- 🔄 AI analysis pipeline

### Phase 2: Advanced Features
- Multi-tenant architecture
- Advanced AI models
- Real-time collaboration
- Mobile application

### Phase 3: Enterprise
- Enterprise SSO integration
- Advanced reporting
- Workflow automation
- API marketplace

## 🌟 Acknowledgments

- Microsoft Semantic Kernel team
- Azure AI services
- .NET Community
- Healthcare AI research community

---

**⚠️ Important**: This application handles Protected Health Information (PHI). Ensure all deployments comply with HIPAA regulations and your organization's security policies.
#include "HrFramework/Context/HrContext.h"
#include "HrFramework/Binder/HrSignalCommandBinder.h"
#include "HrFramework/Binder/HrMediationBinder.h"

using namespace Hr;

HrContext::HrContext()
{
}

HrContext::~HrContext()
{
}

void HrContext::MapBindings()
{
	this->BindCommand<HrSignalCommandBinder>();
	this->BindMediator<HrMediationBinder>();
}

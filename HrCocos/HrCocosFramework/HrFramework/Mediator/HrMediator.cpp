#include "HrFramework/Mediator/HrMediator.h"
#include "HrFramework/View/HrView.h"

using namespace Hr;

HrMediator::HrMediator():m_pView(nullptr)
{
}

HrMediator::~HrMediator()
{

}

void HrMediator::BindView(HrView* pView)
{
	m_pView = pView;
	
	OnRegister();
}

void HrMediator::OnRegister()
{
}


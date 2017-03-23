#include "HrExamples/HrHelloWorldMediator.h"
#include "HrExamples/HrHelloWorldView.h"
#include <boost/assert.hpp>

using namespace Hr;

HrHelloWorldMediator::HrHelloWorldMediator()
{
}

HrHelloWorldMediator::~HrHelloWorldMediator()
{

}

void HrHelloWorldMediator::OnRegister()
{
	m_pHelloWorldView = dynamic_cast<HrHelloWorldView*>(m_pView);
	m_pHelloWorldView->GetClickBtnSignal()->Register(HR_CALLBACK_1(HrHelloWorldMediator::OnClickBtn, this), this, "OnClickBtn");
}

void HrHelloWorldMediator::OnClickBtn(const HrParamPtr& param)
{
	CCLOG("OnClickBtn!!!!!");
}

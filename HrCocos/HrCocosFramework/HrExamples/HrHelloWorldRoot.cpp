#include "HrHelloWorldRoot.h"
#include "HrHelloWorldContext.h"

using namespace Hr;

HrHelloWorldRoot::HrHelloWorldRoot()
{

}

HrHelloWorldRoot::~HrHelloWorldRoot()
{

}
void HrHelloWorldRoot::Init()
{
	HrFrameRoot::Init();

	m_pContext = std::make_shared<HrHelloWorldContext>();
}

void HrHelloWorldRoot::Start()
{
	HrFrameRoot::Start();
}

